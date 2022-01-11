using UnityEngine;
using Photon.Pun;

public class PlayerCameraFollow : MonoBehaviourPunCallbacks
{

    [SerializeField]
    PhotonView view;

    #region Private Fields

    [Tooltip("The distance in the local x-z plane to the target")]
    [SerializeField]
    private float distance = 7.0f;

    [Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
    [SerializeField]
    private Vector3 centerOffset = Vector3.zero;

    [Tooltip("The Smoothing for the camera to follow the target")]
    [SerializeField]
    private float smoothSpeed = 0.125f;

    [SerializeField]
    private Camera PlayerCam;

    // cached transform of the target
    Transform cameraTransform;

    // maintain a flag internally to reconnect if target is lost or camera is switched
    //bool isFollowing;

    // Cache for camera offset
    Vector3 cameraOffset = Vector3.zero;


    #endregion


    #region MonoBehaviour Callbacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase
    /// </summary>
    void Start()
    {
        //PlayerCam = Camera.main;
        PlayerCam.orthographicSize = 15;
        // Start following the target if wanted.
        view = GetComponent<PhotonView>();
        
        //if (followOnStart)
        //{
        //    OnStartFollowing();
        //}
        if (!view.IsMine)
        {
            PlayerCam.enabled = false;
        }
    }


    void Update()
    {
        if(view.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
    }


    #endregion


    #region Public Methods

    public void OnStartFollowing()
    {
        cameraTransform = PlayerCam.transform;
        Cut();
    }

    public void SetCamSizeWipe()
    {
        PlayerCam.orthographicSize = 15;
    }

    public void CamSizeChange(int argSize)
    {
        float t = 0;
        int startingSize = 10 + ( 5 + (argSize - 1));
        int endSize = 10 + (5 + argSize);

        PlayerCam.orthographicSize = Mathf.SmoothStep(startingSize, endSize, t);
    }
    #endregion


    #region Private Methods


    /// <summary>
    /// Follow the target smoothly
    /// </summary>
    void Follow()
    {
        cameraOffset.z = -distance;
        //cameraOffset.y = height;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position + this.transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);
    }


    void Cut()
    {
        cameraOffset.z = -distance;
        //cameraOffset.y = height;
        cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);
        //cameraTransform.LookAt(this.transform.position + centerOffset);
    }
    #endregion
}
