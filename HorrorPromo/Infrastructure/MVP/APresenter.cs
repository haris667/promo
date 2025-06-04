using UnityEngine;

namespace Infrastructure.MVP 
{
    public abstract class APresenter<Model, View>
        where Model : AModel
        where View : AView
    {
        protected Model _model;
        protected View _view;

        public APresenter(Model model, View view)
        {
            _model = model;
            _view = view;
        }

        public APresenter(Model model)
        {
            _model = model;
        }

        public void SetView(View view)
        {
            _view = view;
        }

        public abstract void SetConfigToModel(ScriptableObject config);

        public virtual void Show() => _view.Show();

        public virtual void Hide() => _view.Hide();
    }
}