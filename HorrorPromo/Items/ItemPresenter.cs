using Infrastructure.MVP;
using UnityEngine;

namespace Items
{

    //для итемов вышло избыточно юзать весь MVP. На данный момент итемы ограничиваются вью слоем
    //и хоть, в некоторых есть бизнес-логика (радио меняет состояние), делать кашу всех предметов в одном презентере я не хочу
    //да и удобнее монобех настраивать. Думаю, если получу оффер, то предприму с этим что-то и вся логика будет на презентер слое
    //ибо в таком варианте логика от данных не разделяется :(
    public class ItemPresenter : APresenter<ItemModel, ItemView>
    {
        public ItemPresenter(ItemModel model) : base(model) { }

        public override void SetConfigToModel(ScriptableObject config)
        {
            if (config is ItemConfig itemConfig)
                _model.Init(itemConfig);
            else
                Debug.LogError("Wrong config type!");
        }

        
    }
}