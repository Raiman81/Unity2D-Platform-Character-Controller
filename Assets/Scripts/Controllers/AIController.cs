using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    [CreateAssetMenu(fileName = "AIController", menuName = "InputController/AIController")]
    public class AIController : InputController
    {
        [Header("Interaction")] 
        [SerializeField] private LayerMask layerMask = -1;

        [Header("Ray")] 
        [SerializeField] private float bottomDistance = 1f;
        [SerializeField] private float topDistance = 1f;
        [SerializeField] private float xOffset = 1f;

        private RaycastHit2D _groundInfoBottom;
        private RaycastHit2D _groundInfoTop;
        
        public override bool RetrieveJumpInput(GameObject gameObject)
        {
            return false;
        }

        public override bool RetrieveJumpHoldInput(GameObject gameObject) => false;

        public override float RetrieveMoveInput(GameObject gameObject)
        {
            _groundInfoBottom =
                Physics2D.Raycast(
                    new Vector2(gameObject.transform.position.x + (xOffset * gameObject.transform.localScale.x),
                        gameObject.transform.position.y), Vector2.down, bottomDistance, layerMask);


            _groundInfoTop =
                Physics2D.Raycast(
                    new Vector2(gameObject.transform.position.x + (xOffset * gameObject.transform.localScale.x),
                        gameObject.transform.position.y), Vector2.right * gameObject.transform.localScale.x,
                    topDistance, layerMask); 
            

            if (_groundInfoTop.collider == true || _groundInfoBottom.collider == false)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1,
                    gameObject.transform.localScale.y);
            }

            return gameObject.transform.localScale.x;
        }
    }
}
