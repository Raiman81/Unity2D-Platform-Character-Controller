using UnityEngine;

namespace Controllers
{
    [CreateAssetMenu(fileName = "EmptyController", menuName = "InputController/EmptyController")]
    public class EmptyController : InputController
    {
        public override float RetrieveMoveInput(GameObject gameObject) => 0f;

        public override bool RetrieveJumpInput(GameObject gameObject) => false;

        public override bool RetrieveJumpHoldInput(GameObject gameObject) => false;
    }
}
