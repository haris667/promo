namespace Core.MVP 
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

        public virtual void Show() => _view.Show();

        public virtual void Hide() => _view.Hide();
    }
}