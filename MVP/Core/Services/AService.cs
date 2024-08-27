using Core.Commands;
using UnityEngine;

namespace Core.Services 
{
    /// <summary>
    /// Выполняет роль базового класса для сервисов. Наследуемся от него и пишем свой сервис для чего-либо.
    /// Если методов не хватает расширяем путем перегрузки. Если есть какие-либо технические ограничения то
    /// реализуем интерфейс IService и вводим параллельный функционал сервисов.
    /// </summary>
    /// <typeparam name="Command"></typeparam>
    public abstract class AService<Command> : IService where Command : ICommand
    {
        protected Command _command;

        public AService(Command command) => _command = command;

        public void SetCommand(Command command) => _command = command;

        /// <summary>
        /// Вызов выполнения сервиса, путем перегрузки можно прокидывать и возвращать различные данные.
        /// </summary>
        public virtual void Execute() { }

        public virtual Vector3 Execute(Vector3 vector) { return vector; }
    }
}