using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;
using Spine;
using System.Collections;
using Animation = UnityEngine.Animation;

namespace Nameofthegame.Inputs
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(Animator))]
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private GameObject muzzle;
        [SerializeField] private Image gunImage;
        [SerializeField] private TMP_Text ammoText;
        [SerializeField] private SkeletonMecanim skeletonMecanim;
        [SerializeField] private MakeHitScript makeHit;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private int animLayer = 1;

        [Header("Reload settings")]
        [SerializeField] private int ammo;
        [SerializeField] private int magazineSize;
        [SerializeField] private int currentMagazineAmmo;
        [SerializeField] private AnimationClip reloadClip;
        [SerializeField] private AnimationClip fireClip;
        [Header("Bullet case settings")]
        [SerializeField] private bool hasBulletCases;
        [SerializeField] private GameObject casePrefab;
        [SerializeField] private float caseSpeed;
        [SerializeField] private float caseDelay;
        [SerializeField] private float caseLifetime;
        [SerializeField] private SyncronizeAnimation syncronizeAnimation;

        private bool canFire = true;
        private Spine.Skeleton skeleton;
        private SkeletonDataAsset skeletonDataAsset;
        private Spine.SkeletonData skeletonData;
        private Spine.BoneData boneData;
        private Bone bone;
        private IEnumerator coroutine;
        private float reloadDelay;
        private float fireDelay;
        private bool isFiring;
        private Animator animator;
        private Animator muzzleAnimator;
        private Transform reloadSign;
        private UnityEngine.Animation reloadSignAnimation;
        private bool isReloading;

        private void Awake()
        {
            if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
            if (skeletonMecanim == null) skeletonMecanim = GetComponent<SkeletonMecanim>();
            animator = GetComponent<Animator>();
            if (muzzle != null) muzzleAnimator = muzzle.GetComponent<Animator>();
            if (makeHit != null) makeHit = GetComponent<MakeHitScript>();
            reloadSign = gunImage.transform.GetChild(0);
            reloadSignAnimation = reloadSign.GetComponent<Animation>();
            skeleton = skeletonMecanim.skeleton;
            bone = skeleton.FindBone("pistol");
            skeletonDataAsset = skeletonMecanim.skeletonDataAsset;
            skeletonData = skeletonDataAsset.GetSkeletonData(true);
            boneData = skeletonData.FindBone("pistol");
            reloadDelay = reloadClip.length;
            fireDelay = fireClip.length;
        }

        private void Update()
        {
            animator.SetBool("Shoot", false);
            muzzleAnimator.SetBool("Shoot", false);
            UpdateWeaponInfo();
            isFiring = Input.GetButtonDown(GameNamespace.FIRE1);
            //muzzle.transform.TransformPoint( new Vector3(bone.WorldX, bone.WorldY,transform.position.z));
            if (isFiring && !animator.GetCurrentAnimatorStateInfo(1).IsName("Shoot") && !(animator.GetBool("Edge"))) CalculateFirePossibility();
        }

        private void UpdateWeaponInfo()
        {
            ammoText.text = currentMagazineAmmo.ToString() + "/" + ammo;
        }

        private Vector2 GetBonePosition(Bone someBone)
        {
            float tipX;
            float tipY;
            someBone.LocalToWorld(someBone.Data.Length, 0, out tipX, out tipY);
            Vector2 vector2 = new Vector2(tipX, tipY);
            return (vector2);
        }

        private void CalculateFirePossibility()
        {
            if (currentMagazineAmmo > 0)
            {
                FireTheWeapon();
            }
            else
            {
                if (!isReloading) CalculateReloading();
            }
        }

        private void FireTheWeapon()
        {
            canFire = false;
            //Debug.Log("haha");
            if (currentMagazineAmmo > 0) currentMagazineAmmo--;
            animator.SetBool("Shoot", true);
            muzzleAnimator.SetBool("Shoot", true);
            makeHit.CreateAttackCollider(playerMovement.isRight);
            if (hasBulletCases)
            {
                GameObject bulletCase = Instantiate(casePrefab, muzzle.transform.position, Quaternion.identity);
                BulletCaseBehaviour bulletCaseBehaviour = bulletCase.GetComponent<BulletCaseBehaviour>();
            }
        }

        private void CalculateReloading()
        {
            if (ammo > magazineSize)
            {
                Reloading(magazineSize);
            }
            else
            {
                if (ammo > 0)
                {
                    Reloading(ammo);
                }
            }
        }

        private void Reloading(int reloadAmmo)
        {
            coroutine = ReloadingTime(reloadDelay, reloadAmmo);
            animator.SetBool("Reload", true);
            StartCoroutine(coroutine);
        }

        private IEnumerator ReloadingTime(float waitTime, int reloadAmmo)
        {
            isReloading = true;
            reloadSign.gameObject.SetActive(true);
            syncronizeAnimation.Syncronizing = true;
            yield return new WaitForSeconds(waitTime);
            isReloading = false;
            reloadSign.gameObject.SetActive(false);
            syncronizeAnimation.Syncronizing = false;
            currentMagazineAmmo = reloadAmmo;
            ammo -= reloadAmmo;
            animator.SetBool("Reload", false);
        }

    }
}