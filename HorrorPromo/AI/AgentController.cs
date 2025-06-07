using Cysharp.Threading.Tasks;
using Quests;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AI{

    public class AgentController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;

        // Параметры аниматора
        [Header("Animation Parameters")]
        [SerializeField] private string _speedParam = "Speed";
        [SerializeField] private string _directionXParam = "DirectionX";
        [SerializeField] private string _directionYParam = "DirectionY";
        [SerializeField] private float _animationSmoothTime = 0.1f;

        private bool _isMoving;
        private Quaternion _targetRotation;
        private Vector2 _smoothDeltaPosition = Vector2.zero;
        private Vector2 _velocity = Vector2.zero;
        private Transform _transform;
        private Vector3 _finalLookDirection;

        private void Awake()
        {
            _transform = transform;

            // Отключаем автоматическое обновление позиции для ручного управления
            if (_agent != null)
            {
                _agent.updatePosition = false;
                _agent.updateRotation = false;
            }
        }

        private void Update()
        {
            if (_animator == null || _agent == null) return;

            // Рассчитываем движение для blend tree
            Vector3 worldDeltaPosition = _agent.nextPosition - _transform.position;

            // Преобразуем в локальное пространство
            Vector3 localDelta = _transform.InverseTransformDirection(worldDeltaPosition);
            localDelta.y = 0;

            // Сглаживаем дельту
            float deltaTime = Time.deltaTime;
            Vector2 deltaPosition = new Vector2(localDelta.x, localDelta.z);
            _smoothDeltaPosition = Vector2.SmoothDamp(
                _smoothDeltaPosition,
                deltaPosition,
                ref _velocity,
                _animationSmoothTime,
                Mathf.Infinity,
                deltaTime
            );

            // Рассчитываем скорость для blend tree (0-1)
            float speed = _smoothDeltaPosition.magnitude / deltaTime;
            float normalizedSpeed = Mathf.Clamp01(speed / _agent.speed);

            // Устанавливаем параметры аниматора
            _animator.SetFloat(_speedParam, normalizedSpeed, _animationSmoothTime, deltaTime);
            _animator.SetFloat(_directionXParam, _smoothDeltaPosition.x / deltaTime, _animationSmoothTime, deltaTime);
            _animator.SetFloat(_directionYParam, _smoothDeltaPosition.y / deltaTime, _animationSmoothTime, deltaTime);

            // Синхронизация позиции
            if (worldDeltaPosition.magnitude > _agent.radius)
            {
                _transform.position = _agent.nextPosition - 0.9f * worldDeltaPosition;
            }

            // Поворот в сторону движения во время перемещения
            if (_isMoving && _agent.velocity.sqrMagnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_agent.velocity.normalized);
                _transform.rotation = Quaternion.RotateTowards(
                    _transform.rotation,
                    targetRotation,
                    _agent.angularSpeed * Time.deltaTime
                );
            }
        }

        public async UniTask ExecuteAgentEvent(AgentEventSettings settings)
        {
            if (_agent == null) return;

            // Сохраняем финальное направление для поворота после достижения точки
            _finalLookDirection = settings.lookDirection;

            // Настройка параметров агента
            _agent.speed = settings.moveSpeed;
            _agent.angularSpeed = settings.rotationSpeed;
            _agent.stoppingDistance = settings.stoppingDistance;

            // Начало движения
            _agent.isStopped = false;
            _agent.SetDestination(settings.targetPosition);
            _isMoving = true;

            // Ожидание достижения цели
            await WaitForDestination();

            // Сброс параметров движения
            _animator.SetFloat(_speedParam, 0);
            _animator.SetFloat(_directionXParam, 0);
            _animator.SetFloat(_directionYParam, 0);

            // Поворот в финальное направление
            await RotateToFinalDirection();

            // Запуск финальной анимации
            if (!string.IsNullOrEmpty(settings.animationName))
            {
                await UniTask.Delay((int)(settings.delayBeforeAction * 1000));
                _animator.SetTrigger(settings.animationName);
            }
        }

        private async UniTask WaitForDestination()
        {
            while (_isMoving)
            {
                // Проверка достижения цели
                if (!_agent.pathPending &&
                    _agent.remainingDistance <= _agent.stoppingDistance &&
                    (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f))
                {
                    _isMoving = false;
                    _agent.isStopped = true;
                    break;
                }

                await UniTask.Yield();
            }
        }

        private async UniTask RotateToFinalDirection()
        {
            if (_finalLookDirection == Vector3.zero) return;

            _targetRotation = Quaternion.LookRotation(_finalLookDirection.normalized);
            float angleThreshold = 1f;

            while (Quaternion.Angle(_transform.rotation, _targetRotation) > angleThreshold)
            {
                _transform.rotation = Quaternion.RotateTowards(
                    _transform.rotation,
                    _targetRotation,
                    _agent.angularSpeed * Time.deltaTime
                );
                await UniTask.Yield();
            }
        }

        void OnAnimatorMove()
        {
            // Синхронизация позиции с анимацией корневого движения
            if (_agent != null && _animator != null)
            {
                Vector3 position = _animator.rootPosition;
                position.y = _agent.nextPosition.y;
                _transform.position = position;
                _agent.nextPosition = _transform.position;
            }
        }

        void OnDrawGizmosSelected()
        {
            if (_isMoving && _agent != null && _agent.hasPath)
            {
                // Визуализация пути
                Gizmos.color = Color.cyan;
                for (int i = 0; i < _agent.path.corners.Length - 1; i++)
                {
                    Gizmos.DrawLine(_agent.path.corners[i], _agent.path.corners[i + 1]);
                    Gizmos.DrawSphere(_agent.path.corners[i], 0.1f);
                }
                Gizmos.DrawSphere(_agent.path.corners[_agent.path.corners.Length - 1], 0.2f);

                // Визуализация финального направления
                if (_finalLookDirection != Vector3.zero)
                {
                    Gizmos.color = Color.red;
                    Vector3 endPoint = _transform.position + _finalLookDirection.normalized * 2f;
                    Gizmos.DrawLine(_transform.position, endPoint);
                    Gizmos.DrawSphere(endPoint, 0.15f);
                }
            }
        }
    }
}


