using Installers;
using Quests;
using System;
using UnityEngine;
using Zenject;

namespace Horror
{
    public class GameEventController : IInitializable, IDisposable
    {
        [Inject] 
        private SignalBus _signalBus;
        [SerializeField]
        private EventExecuter _executer;
        private EventConfig _config;

        public GameEventController(EventConfig config, EventExecuter executer)
        {
            _config = config;
            _executer = executer;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<QuestTaskProgressSignal>(OnQuestTaskProgress);
            _signalBus.Subscribe<QuestStartedSignal>(OnQuestStarted);
            _signalBus.Subscribe<QuestCompletedSignal>(OnQuestCompleted);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<QuestTaskProgressSignal>(OnQuestTaskProgress);
            _signalBus.Unsubscribe<QuestStartedSignal>(OnQuestStarted);
            _signalBus.Unsubscribe<QuestCompletedSignal>(OnQuestCompleted);
        }

        private void OnQuestTaskProgress(QuestTaskProgressSignal signal)
        {
            foreach (var gameEvent in _config.events)
            {
                if (gameEvent.triggerType == TriggerType.TaskProgress && 
                    gameEvent.questId == signal.questId && 
                    gameEvent.taskId == signal.activeTaskId &&
                    gameEvent.progressTrigger == signal.progress)
                {
                    ExecuteHorrorEvent(gameEvent);
                }
            }
        }

        private void OnQuestStarted(QuestStartedSignal signal)
        {
            foreach (var gameEvent in _config.events)
            {
                if (gameEvent.triggerType == TriggerType.QuestStart &&
                    gameEvent.questId == signal.questId)
                {
                    ExecuteHorrorEvent(gameEvent);
                }
            }
        }

        private void OnQuestCompleted(QuestCompletedSignal signal)
        {
            foreach (var gameEvent in _config.events)
            {
                if (gameEvent.triggerType == TriggerType.QuestComplete &&
                    gameEvent.questId == signal.questId)
                {
                    ExecuteHorrorEvent(gameEvent);
                }
            }
        }

        //выделить в отдельный сервис. Аля экзекутер
        private void ExecuteHorrorEvent(GameEvent gameEvent)
        {
            _executer.ExecuteEvent(gameEvent);
        }
    }

}
