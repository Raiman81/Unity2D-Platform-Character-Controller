using UnityEngine;

namespace Controllers
{
    [CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
    public class PlayerController : InputController
    {
        public override bool RetrieveJumpInput(GameObject gameObject) => Input.GetButtonDown("Jump");

        public override bool RetrieveJumpHoldInput(GameObject gameObject) => Input.GetButton("Jump");

        public override float RetrieveMoveInput(GameObject gameObject) => Input.GetAxisRaw("Horizontal");
    }
}
