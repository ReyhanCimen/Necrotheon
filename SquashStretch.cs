using UnityEngine;

public class SkeletonWobble : MonoBehaviour
{
    public float frequency = 2f;         
    public float scaleAmount = 0.2f;     
    public float speed = 2f;             
    public Transform player;           

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

   
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null)
            return;
  
transform.forward = Camera.main.transform.forward;

 
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

    
        if (direction.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);


        float time = Time.time * frequency * Mathf.PI * 2;
        float xScale = 1 + Mathf.Sin(time) * scaleAmount;
        float yScale = 1 - Mathf.Sin(time) * scaleAmount;

       
        float directionSign = Mathf.Sign(transform.localScale.x); 
        transform.localScale = new Vector3(directionSign * originalScale.x * xScale, originalScale.y * yScale, originalScale.z);
    }
}