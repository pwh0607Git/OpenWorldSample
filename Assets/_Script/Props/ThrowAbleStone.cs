using UnityEngine;
using DG.Tweening;

public class ThrowAbleStone : MonoBehaviour
{    
    [SerializeField] GameObject target;
    [SerializeField] GameObject subEmission;
    [SerializeField] ParticleSystem particle;
    Rigidbody rb;
    private bool isTaken;           // 현재 골렘의 손에 쥐어져 있는지...
    void Start(){
        Init();
    }

    void Init(){
        particle = GetComponentInChildren<ParticleSystem>();
        subEmission = GetComponentInChildren<SubEmissionMaker>().gameObject;
        particle.gameObject.SetActive(false);
        subEmission.SetActive(false);
        target = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        isTaken = true;
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    [SerializeField] float duration;
    [SerializeField] float maxHeight;
    public Ease easeType;

    // Boss의 animation event를 통해 해당 메서드 호출됨
    public void Throw(){
        //easeType = Sin
        isTaken = false;
        transform.DOJump(target.transform.position, maxHeight, 1,duration).SetEase(easeType);
    }
    void OnTriggerEnter(Collider other) {
        if(isTaken) return;     //손에 쥐어진 상태라면 무시.

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
        particle.gameObject.SetActive(true);
        particle.Play();
        particle.transform.SetParent(null);
    }

    public void MakeSubEmission(){
        subEmission.SetActive(true);
        subEmission.transform.SetParent(null);
    }
}