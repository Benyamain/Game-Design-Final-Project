namespace Fusion.Menu {
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using TMPro;
  using UnityEngine;
  using UnityEngine.UI;

  /// <summary>
  /// The party screen shows two modes. Creating a new game or joining a game with a party code.
  /// After creating a game the session party code can be optained via the ingame menu.
  /// One speciality is that a region list is requested from the connection when entering the screen in order to create a matching session codes.
  /// </summary>
  public partial class PhotonMenuUIGameMode : PhotonMenuUIScreen {
    [InlineHelp, SerializeField] protected Button _selectButton;
    /// <summary>
    /// The join game button.
    /// </summary>
    [InlineHelp, SerializeField] protected Button _backButton;
    /// <summary>
    /// The scene thumbnail. Can be null.
    /// </summary>
    [InlineHelp, SerializeField] protected Image _sceneThumbnail;

    /// <summary>
    /// Callback fired before the connection is created.
    /// This can stop the connection attempt with an <see cref="ConnectResult"/>.
    /// </summary>
    public Func<IPhotonMenuConnectArgs, Task<ConnectResult>> OnBeforeConnection;

    /// <summary>
    /// The task of requesting the regions.
    /// </summary>
    protected Task<List<PhotonMenuOnlineRegion>> _regionRequest;

    partial void AwakeUser();
    partial void InitUser();
    partial void ShowUser();
    partial void HideUser();
    partial void BeforeConnectUser();

    /// <summary>
    /// The Unity awake method. Calls partial method <see cref="AwakeUser"/> to be implemented on the SDK side.
    /// </summary>
    public override void Awake() {
      base.Awake();
      AwakeUser();
    }

    /// <summary>
    /// The screen init method. Calls partial method <see cref="InitUser"/> to be implemented on the SDK side.
    /// </summary>
    public override void Init() {
      base.Init();
      InitUser();
    }

    /// <summary>
    /// The screen show method. Calls partial method <see cref="ShowUser"/> to be implemented on the SDK side.
    /// When entering this screen an async request to retrieve the available regions is started.
    /// </summary>
    public override void Show() {
      base.Show();

      if (Config.CodeGenerator == null) {
        Debug.LogError("Add a CodeGenerator to the PhotonMenuConfig");
      }

      if (_regionRequest == null || _regionRequest.IsFaulted) {
        // Request the regions already when entering the party menu
        _regionRequest = Connection.RequestAvailableOnlineRegionsAsync(ConnectionArgs);
      }

      if (_sceneThumbnail != null) {
        if (ConnectionArgs.Scene.Preview != null) {
          _sceneThumbnail.transform.parent.gameObject.SetActive(true);
          _sceneThumbnail.sprite = ConnectionArgs.Scene.Preview;
          _sceneThumbnail.gameObject.SendMessage("OnResolutionChanged");
        } else {
          _sceneThumbnail.transform.parent.gameObject.SetActive(false);
          _sceneThumbnail.sprite = null;
        }
      }

      ShowUser();
    }

    /// <summary>
    /// The screen hide method. Calls partial method <see cref="HideUser"/> to be implemented on the SDK side.
    /// </summary>
    public override void Hide() {
      base.Hide();
      HideUser();
    }

    /// <summary>
    /// Is called when the <see cref="_backButton"/> is pressed using SendMessage() from the UI object.
    /// </summary>
    public virtual void OnBackButtonPressed() {
      Controller.Show<PhotonMenuUIMain>();
    }
  }
}