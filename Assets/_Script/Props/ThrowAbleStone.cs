using UnityEngine;

public class ThrowAbleStone : MonoBehaviour
{    
    [SerializeField] GameObject target;
    [SerializeField] GameObject particle;
    [SerializeField] GameObject subEmission;
    Vector3 startPosition;
    [SerializeField] float addition;
    void Start(){
        startPosition = transform.position;
        particle = GetComponentInChildren<ParticleSystem>().gameObject;
        particle.SetActive(false);
        subEmission = GetComponentInChildren<SubEmissionHandler>().gameObject;
        subEmission.SetActive(false);
        target = GameObject.FindWithTag("Player");
        Shoot();
    }

    void Update(){   
    }

    void Shoot()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) return;

        Vector3 startPos = transform.position;
        Vector3 targetPos = target.transform.position;

        float height = 5f; // 포물선 최고점 높이
        float gravity = Physics.gravity.magnitude;

        // 1️⃣ XZ 평면 거리 계산
        Vector3 displacementXZ = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z);
        float distanceXZ = displacementXZ.magnitude;

        // 2️⃣ Y축(높이) 운동 계산
        float timeToPeak = Mathf.Sqrt(2 * height / gravity);  // 최고점까지 가는 시간
        float timeToTarget = timeToPeak + Mathf.Sqrt(2 * (targetPos.y - startPos.y + height) / gravity);  // 전체 이동 시간

        // 3️⃣ 초기 속도 계산
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(2 * gravity * height);  // Y축 속도
        Vector3 velocityXZ = displacementXZ / timeToTarget;  // XZ축 속도

        // 4️⃣ 최종 속도 적용
        rb.velocity = velocityXZ + velocityY;
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Floor" || other.gameObject.tag == "Player"){
            Debug.Log($"{other.gameObject.name}과 충돌!! Destroy");
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        ShowParticle();
        MakeSubEmission();
    }

    public void ShowParticle(){
        particle.SetActive(true);
        particle.transform.SetParent(null);         // 파편 추출.
    }

    public void MakeSubEmission(){
        subEmission.SetActive(true);
        subEmission.transform.SetParent(null);
    }
}