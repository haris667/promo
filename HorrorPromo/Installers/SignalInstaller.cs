using Items;
using UniRx;
using UnityEngine;
using Zenject;

namespace Installers
{
    //захотел связать презентеры чем-то новым для себя и выбрал сигналы
    //на самом деле хз, насколько они хороши, зенжект и так избыточен, но
    //раз такое есть, почему бы и не потыкаться
    public class SignalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<ItemAddedToInventorySignal>();
            Container.DeclareSignal<ItemRemovedToInventorySignal>();
            Container.DeclareSignal<ItemUsedSignal>();
            Container.DeclareSignal<ItemBaseSignal>();

            Container.DeclareSignal<AimItemSignal>();
            Container.DeclareSignal<AimVoidSignal>();

            Container.DeclareSignal<QuestTaskProgressSignal>();
            Container.DeclareSignal<QuestStartedSignal>();
            Container.DeclareSignal<QuestCompletedSignal>();

            Container.DeclareSignal<FlickerLightsSignal>();
            Container.DeclareSignal<RadioStaticSignal>();
            Container.DeclareSignal<ChangeWaterColorSignal>();
            Container.DeclareSignal<ClientOrderSignal>();
            Container.DeclareSignal<CameraShakeSignal>();
            Container.DeclareSignal<PlaySoundSignal>();

            Container.DeclareSignal<SubtitleStartSignal>();
            Container.DeclareSignal<SubtitleEndSignal>();

            Container.DeclareSignal<PlayerInsideCoordSignal>();
            Container.DeclareSignal<PlayerOutsideCoordSignal>();

            Container.DeclareSignal<DestroyItemSignal>();

            Container.DeclareSignal<CloseInventorySignal>();
        }
    }

    public struct ItemAddedToInventorySignal
    {
        public ItemConfig item;
        public int amount;
        public Vector3 position;
        public AudioClip clip;
    }

    public struct AimItemSignal
    {
        public AudioClip clip;
    }

    public struct AimVoidSignal { }

    public struct SubtitleStartSignal { }

    public struct SubtitleEndSignal { }

    public struct ItemRemovedToInventorySignal
    {
        public string id;
        public int amount;
        public Vector3 position;
        public AudioClip clip;
    }

    //где колбэк на обратку - классы, чтобы было понятнее 
    public class PlayerInsideCoordSignal 
    {
        public ItemConfig item;
        public int amount;
        public ReactiveProperty<bool> destroySelfTrigger;
    }

    public struct PlayerOutsideCoordSignal 
    {
        public ItemConfig item;
        public int amount;
    }


    public struct ItemUsedSignal
    {
        public ItemConfig item;
        public AudioClip clip;
        public int amount;
    }

    //ультимативный сигнал со всей нужной инфой
    //пока маленький норм, будет больше - хреново, нужно будет дробить как выше
    public struct ItemBaseSignal
    {
        public ItemConfig item;
        public string id;
        public int amount;
        public Vector3 position;
    }

    public struct QuestTaskProgressSignal
    {
        public string questId;
        public string activeTaskId; //== itemId
        public int progress;
    }

    public struct QuestStartedSignal
    {
        public string questId;
    }

    public struct QuestCompletedSignal
    {
        public string questId;
    }

    public struct FlickerLightsSignal
    {
        public float duration;
    }

    public struct RadioStaticSignal
    {
        public float duration;
    }

    public struct ChangeWaterColorSignal
    {
        public Color color;
    }

    public struct ClientOrderSignal
    {
        public string orderText;
    }

    public struct CameraShakeSignal
    {
        public float intensity;
        public float duration;
    }

    public struct PlaySoundSignal
    {
        public string sound;
    }

    public struct DestroyItemSignal
    {
        public ItemConfig config;
    }

    public struct CloseInventorySignal { }
}