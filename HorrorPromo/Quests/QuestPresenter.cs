using Cysharp.Threading.Tasks;
using Infrastructure.MVP;
using Installers;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests
{
    //можно масштабировать до одновременно n активных квестов, возможно, сделаю это позже
    public class QuestPresenter : APresenter<QuestModel, QuestView>, IInitializable, IDisposable
    {
        [Inject] 
        private SignalBus _signalBus;
        private ActiveQuest _activeQuest;

        public QuestPresenter(QuestModel model) : base(model)
        {
            _model = model;
        }

        public void Initialize()
        {
            _activeQuest = new ActiveQuest(_model.allQuests.First(quest => quest.autoActivate == true), _signalBus);
            UpdateTaskTextView();

            _signalBus.Subscribe<ItemAddedToInventorySignal>(HandleItemAddedToInventorySignal);
            _signalBus.Subscribe<ItemRemovedToInventorySignal>(HandleItemRemovedToInventorySignal);
            _signalBus.Subscribe<ItemUsedSignal>(HandleItemUsedSignal);
            _signalBus.Subscribe<PlayerInsideCoordSignal>(HandlePlayerInsideCoordSignal);
            _signalBus.Subscribe<DestroyItemSignal>(HandleDestroyItemSignal);
            _signalBus.Subscribe<CloseInventorySignal>(HandleCloseInventorySignal);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ItemAddedToInventorySignal>(HandleItemAddedToInventorySignal);
            _signalBus.Unsubscribe<ItemRemovedToInventorySignal>(HandleItemRemovedToInventorySignal);
            _signalBus.Unsubscribe<ItemUsedSignal>(HandleItemUsedSignal);
            _signalBus.Unsubscribe<PlayerInsideCoordSignal>(HandlePlayerInsideCoordSignal);
            _signalBus.Unsubscribe<DestroyItemSignal>(HandleDestroyItemSignal);
            _signalBus.Unsubscribe<CloseInventorySignal>(HandleCloseInventorySignal);
        }

        private void HandleItemAddedToInventorySignal(ItemAddedToInventorySignal signal)
        {
            UpdateQuestProgress(TaskType.Collect, signal.item.id, signal.amount);
        }

        private void HandleDestroyItemSignal(DestroyItemSignal signal)
        {
            UpdateQuestProgress(TaskType.Destroy, signal.config.id, 1);
        }

        private void HandleItemRemovedToInventorySignal(ItemRemovedToInventorySignal signal)
        {
            UpdateQuestProgress(TaskType.Remove, signal.id, signal.amount);
        }

        private void HandleItemUsedSignal(ItemUsedSignal signal)
        {
            UpdateQuestProgress(TaskType.Use, signal.item.id, signal.amount);
        }

        private void HandlePlayerInsideCoordSignal(PlayerInsideCoordSignal signal)
        {
            UpdateQuestProgress(TaskType.InsideCoord, signal.item.id, signal.amount, signal.destroySelfTrigger);
        }

        private void HandleCloseInventorySignal(CloseInventorySignal signal)
        {
            UpdateQuestProgress(TaskType.ButtonClick, "inventory_close", 1);
        }

        public override void SetConfigToModel(ScriptableObject config)
        {
            if (config is QuestRepositoryConfig questRepositoryConfig)
                _model.Init(questRepositoryConfig);
            else
                Debug.LogError("Wrong config type!");
        }

        private void CompleteQuest(ActiveQuest quest)
        {
            _signalBus.Fire(new QuestCompletedSignal { questId = quest.сonfig.questId }); 

            if (!string.IsNullOrEmpty(quest.сonfig.nextQuestId))
            {
                ActivateQuest(quest.сonfig.nextQuestId);
            }
        }

        private void UpdateQuestProgress(TaskType type, string itemId, int amount, ReactiveProperty<bool> trigger = null)
        {
            _activeQuest.UpdateProgress(type, itemId, amount, trigger);

            UpdateTaskTextView();
            if (_activeQuest.isCompleted)
            {                                         
                CompleteQuest(_activeQuest);
                return;
            }
        }

        public async UniTask UpdateTaskTextView()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_model.delayBeforeNewQuest));
            var activeTask = _activeQuest.GetActiveTask();

            if (activeTask != null)
                _view.SetTaskText(_activeQuest.GetActiveTask().name);
            else
                _view.SetTaskText(string.Empty);
        }

        //паблик на случай если сам выбираешь квестовую линию
        //хотя в хоррорах такого обычно нет
        public void ActivateQuest(string questId)
        {
            if (!_model.questsById.TryGetValue(questId, out var config))
                return;

            var activeQuest = new ActiveQuest(config, _signalBus);
            _activeQuest = activeQuest;

            _signalBus.Fire(new QuestStartedSignal { questId = questId });

            UpdateTaskTextView();
        }
    }

    public class ActiveQuest
    {
        public QuestConfig сonfig { get; private set; }
        public bool isCompleted { get; private set; }

        private readonly Dictionary<QuestTask, int> _taskProgress = new();

        private SignalBus _signalBus;


        public ActiveQuest(QuestConfig config, SignalBus signalBus)
        {
            сonfig = config;
            _signalBus = signalBus;

            foreach (var task in config.tasks)
                _taskProgress[task] = 0;
        }

        public QuestTask GetActiveTask()
        {
            return _taskProgress.FirstOrDefault(task => task.Value < task.Key.requiredAmount && task.Key.isDisplay).Key;
        }

        public bool GetStatusActiveTask()
        {
            return GetActiveTask().requiredAmount > GetTaskProgress();
        }

        public string GetActiveTaskId()
        {
            return GetActiveTask().taskId;
        }

        public int GetTaskProgress()
        {
            return _taskProgress[GetActiveTask()];
        }

        public int GetTaskProgress(QuestTask task)
        {
            return _taskProgress[task];
        }

        //система обрабатывает сразу все активные задачи. Если в процессе мы выполним перед активной вторую - это норм, мы перейдем к третьей
        public void UpdateProgress(TaskType type, string itemId, int amount, ReactiveProperty<bool> trigger = null)
        {
            if (isCompleted) return;

            foreach (var task in сonfig.tasks)
            {
                if ((task.hasPriority && task != GetActiveTask()) || _taskProgress[task] >= task.requiredAmount)
                    continue;

                if (task.type == type && task.itemId == itemId)
                {
                    _taskProgress[task] = Mathf.Min(
                        _taskProgress[task] + amount,
                        task.requiredAmount
                    );

                    if (trigger != null && _taskProgress[task] + amount > task.requiredAmount) 
                        trigger.Value = true; //возвратка если нужно. Внутри отправителя сигнала должна быть реактивная подписка на это поле

                    var activeTask = GetActiveTask();

                    if(activeTask != null && task != activeTask && task.isDisplay) 
                        TryUpdateTaskEvent(activeTask);
                    
                    TryUpdateTaskEvent(task);
                }
            }

            CheckCompletion();
        }

        //под ивентом подразумевается игровое событие, а не часть синтаксиса
        private void TryUpdateTaskEvent(QuestTask task)
        {
            _signalBus.Fire(new QuestTaskProgressSignal
            {
                questId = сonfig.questId,
                activeTaskId = task.taskId,
                progress = _taskProgress[task]
            });
        }

        private void CheckCompletion()
        {
            isCompleted = сonfig.tasks.All(task => _taskProgress[task] >= task.requiredAmount);
        }
    }
}


