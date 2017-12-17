using UnityEngine;
using Klak.Math;

namespace Beta
{
    public class Puppet : MonoBehaviour
    {
        #region Body (root) joint

        [Header("Root Joint")]
        [SerializeField] float _bodyNoiseFrequency = 0.2f;
        [SerializeField] float _noiseToBodyPosition = 0.5f;
        [SerializeField] float _noiseToBodyRotation = 45;

        Vector3 CalculateBodyPosition() {
            return _bodyNoise.Vector(0) * _noiseToBodyPosition;
        }

        Quaternion CalculateBodyRotation() {
            return _bodyNoise.Rotation(1, _noiseToBodyRotation);
        }

        #endregion

        #region Neck joint

        [Header("Neck Joint")]
        [SerializeField] float _neckNoiseFrequency = 0.5f;
        [SerializeField] Vector3 _noiseToNeckRotation = Vector3.one * 45;

        Quaternion CalculateNeckRotation() {
            var r = Vector3.Scale(_neckNoise.Vector(2), _noiseToNeckRotation);
            return Quaternion.Euler(r);
        }

        #endregion

        #region Spine joints

        [Header("Spine Joints")]
            [SerializeField] float _spineBend = 20;
        [SerializeField] float _spineNoiseFrequency = 0.15f;
        [SerializeField] Vector3 _noiseToSpineRotation = Vector3.one * 45;

        Quaternion CalculateSpineRotation() {
            var r = Vector3.Scale(_spineNoise.Vector(3), _noiseToSpineRotation);
            r.x += _spineBend;
            return Quaternion.Euler(r);
        }

        #endregion

        #region Hand IK target

        [Header("Hand IK Target")]
        [SerializeField] Vector3 _leftHandTarget = new Vector3(-0.3f, 0, 0.3f);
        [SerializeField] Vector3 _rightHandTarget = new Vector3(0.3f, 0, 0.3f);
        [SerializeField] float _handNoiseFrequency = 0.2f;
        [SerializeField] Vector3 _noiseToHandTarget = Vector3.one * 0.3f;
        [SerializeField] float _noiseToHandRotation = 30;

        Vector3 CalculateHandPosition(bool right) {
            var p = right ? _rightHandTarget : _leftHandTarget;
            p += Vector3.Scale(_handNoise.Vector(right ? 4 : 5), _noiseToHandTarget);
            var neck = _animator.GetBoneTransform(HumanBodyBones.Chest);
            return neck.TransformPoint(p);
        }

        Quaternion CalculateHandRotation(bool right) {
            return _handNoise.Rotation(right ? 6 : 7, _noiseToHandRotation);
        }

        #endregion

        #region Foot IK target

        [Header("Foot IK Target")]
        [SerializeField] Vector3 _leftFootTarget = new Vector3(-0.2f, -0.5f, 0);
        [SerializeField] Vector3 _rightFootTarget = new Vector3(0.2f, -0.5f, 0);
        [SerializeField] float _footNoiseFrequency = 0.2f;
        [SerializeField] Vector3 _noiseToFootTarget = Vector3.one * 0.3f;
        [SerializeField] float _noiseToFootRotation = 45;

        Vector3 CalculateFootPosition(bool right) {
            var p = right ? _rightFootTarget : _leftFootTarget;
            p += Vector3.Scale(_footNoise.Vector(right ? 8 : 9), _noiseToFootTarget);
            var hip = _animator.GetBoneTransform(HumanBodyBones.Hips);
            return hip.TransformPoint(p);
        }

        Quaternion CalculateFootRotation(bool right)
        {
            var r = _footNoise.Value01(right ? 10 : 11) * _noiseToFootRotation;
            return Quaternion.AngleAxis(r, Vector3.right);
        }

        #endregion

        #region MonoBehaviour functions

        Animator _animator;

        NoiseGenerator _bodyNoise;
        NoiseGenerator _neckNoise;
        NoiseGenerator _spineNoise;
        NoiseGenerator _handNoise;
        NoiseGenerator _footNoise;

        Vector3 _leftHandPosition;
        Vector3 _rightHandPosition;
        Vector3 _leftFootPosition;
        Vector3 _rightFootPosition;

        void Start()
        {
            _animator = GetComponent<Animator>();

            _bodyNoise  = new NoiseGenerator(_bodyNoiseFrequency );
            _neckNoise  = new NoiseGenerator(_neckNoiseFrequency );
            _spineNoise = new NoiseGenerator(_spineNoiseFrequency);
            _handNoise  = new NoiseGenerator(_handNoiseFrequency );
            _footNoise  = new NoiseGenerator(_footNoiseFrequency );
        }

        void Update()
        {
            _bodyNoise .Frequency = _bodyNoiseFrequency;
            _neckNoise .Frequency = _neckNoiseFrequency;
            _spineNoise.Frequency = _spineNoiseFrequency;
            _handNoise .Frequency = _handNoiseFrequency;
            _footNoise .Frequency = _footNoiseFrequency;

            _bodyNoise .Step();
            _neckNoise .Step();
            _spineNoise.Step();
            _handNoise .Step();
            _footNoise .Step();

            _leftHandPosition  = CalculateHandPosition(false);
            _rightHandPosition = CalculateHandPosition(true);
            _leftFootPosition  = CalculateFootPosition(false);
            _rightFootPosition = CalculateFootPosition(true);
        }


        void OnAnimatorIK(int layerIndex)
        {
            _animator.bodyPosition = transform.TransformPoint(CalculateBodyPosition());
            _animator.bodyRotation = CalculateBodyRotation() * transform.rotation;

            var spine = CalculateSpineRotation();
            _animator.SetBoneLocalRotation(HumanBodyBones.Spine, spine);
            _animator.SetBoneLocalRotation(HumanBodyBones.Chest, spine);

            var neck = CalculateNeckRotation();
            _animator.SetBoneLocalRotation(HumanBodyBones.Neck, neck);
            _animator.SetBoneLocalRotation(HumanBodyBones.Head, neck);

            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            _animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandPosition);
            _animator.SetBoneLocalRotation(HumanBodyBones.LeftHand, CalculateHandRotation(false));

            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            _animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandPosition);
            _animator.SetBoneLocalRotation(HumanBodyBones.RightHand, CalculateHandRotation(true));

            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            _animator.SetIKPosition(AvatarIKGoal.LeftFoot, _leftFootPosition);
            _animator.SetBoneLocalRotation(HumanBodyBones.LeftFoot, CalculateFootRotation(false));

            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            _animator.SetIKPosition(AvatarIKGoal.RightFoot, _rightFootPosition);
            _animator.SetBoneLocalRotation(HumanBodyBones.RightFoot, CalculateFootRotation(true));
        }

        #endregion
    }
}
