using System;

namespace Utils.SimpleStateMachine
{
    public abstract class SimpleState<IEnumType> : ISimpleState where IEnumType : Enum
    {
        public IEnumType StateType { get; private set; }

        public SimpleState(IEnumType stateType)
        {
            StateType = stateType;
        }

        public abstract void Start();
        public abstract void Update();
        public abstract void End();
    }
}