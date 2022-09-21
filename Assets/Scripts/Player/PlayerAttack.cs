using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Spine;

namespace Nameofthegame.Inputs
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private GameObject muzzle;
        [SerializeField] private Image gunImage;
        [SerializeField] private Text ammoText;
        [SerializeField] private SkeletonMecanim skeletonMecanim;
        [SerializeField] private MakeHitScript makeHit;
        private int ammo;
        private int magazineSize;
        private int currentMagazineAmmo;
        private bool isRight;
        private Spine.Skeleton skeleton;
        private SkeletonDataAsset skeletonDataAsset;
        private Spine.SkeletonData skeletonData;
        private Spine.BoneData boneData;
        private Bone bone;

        private bool isFiring;
        private Animator animator;
        private Animator muzzleAnimator;
        //TODO: взаимодействие c animator и makehit

        private void Awake()
        {
            if (skeletonMecanim == null) skeletonMecanim = GetComponent<SkeletonMecanim>();
            animator = GetComponent<Animator>();
            if (muzzle!=null) muzzleAnimator = muzzle.GetComponent<Animator>();
            if (makeHit != null) makeHit = GetComponent<MakeHitScript>();
            skeleton = skeletonMecanim.skeleton;
            bone = skeleton.FindBone("pistol");
            skeletonDataAsset = skeletonMecanim.skeletonDataAsset;
            skeletonData = skeletonDataAsset.GetSkeletonData(true);
            boneData = skeletonData.FindBone("pistol");
        }

        private void Update()
        {
            isFiring = Input.GetButtonDown(GameNamespace.FIRE1);
            if (isFiring) CalculatingFirePossibility();
            //muzzle.transform.TransformPoint( new Vector3(bone.WorldX, bone.WorldY,transform.position.z));
        }

        private Vector2 GetBonePosition(Bone someBone)
        {
            float tipX;
            float tipY;
            someBone.LocalToWorld(someBone.Data.Length, 0, out tipX, out tipY);
            Vector2 vector2 = new Vector2 (tipX, tipY);
            return (vector2);
            FireTheWeapon();
        }

        private void CalculatingFirePossibility()
        {

        }

        private void FireTheWeapon()
        {
            animator.SetBool("Shoot", true);
            muzzleAnimator.SetBool("Shoot", true);
            makeHit.CreateAttackCollider(isRight);
        }
    }
}