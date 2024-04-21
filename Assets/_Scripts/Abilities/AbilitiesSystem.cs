using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class AbilitiesSystem : MonoBehaviour
{

    [SerializeField] private List<Ability> _rightAbilities;
    [SerializeField] private List<Ability> _leftAbilities;

    [Space]
    [Header("Right")]
    [SerializeField] private InputActionReference _firstRightAbility;
    [SerializeField] private InputActionReference _secondRightAbility;
    [SerializeField] private InputActionReference _thirdRightAbility;

    [Space]
    [Header("Left")]
    [SerializeField] private InputActionReference _firstLeftAbility;
    [SerializeField] private InputActionReference _secondLeftAbility;
    [SerializeField] private InputActionReference _thirdLeftAbility;


    private EchoManager _rightEchoManager;
    private EchoManager _leftEchoManager;

    [Space(16)]
    // contains info like the main camera the controllers...
    public XROrigin xROrigin;
    public Camera MainCamera;
    public CharacterController controller;
    public InputActionReference RightThumbStick;
    public InputActionReference LeftThumbStick;
    public ActionBasedControllerManager RightController;
    public ActionBasedControllerManager LeftController;

    private void OnEnable()
    {
        EventSystem.Events.OnEchoManagerStart += GetEchoManager;

        _firstRightAbility.action.Enable();
        _firstRightAbility.action.started += StartFirstRightAbility;
        _firstRightAbility.action.canceled += CancelFirstRightAbility;

        _secondRightAbility.action.Enable();
        _secondRightAbility.action.started += StartSecondRightAbility;
        _secondRightAbility.action.canceled += CancelSecondRightAbility;

        _thirdRightAbility.action.Enable();
        _thirdRightAbility.action.started += StartThirdRightAbility;
        _thirdRightAbility.action.canceled += CancelThirdRightAbility;

        _firstLeftAbility.action.Enable();
        _firstLeftAbility.action.started += StartFirstLeftAbility;
        _firstLeftAbility.action.canceled += CancelFirstLeftAbility;

        _secondLeftAbility.action.Enable();
        _secondLeftAbility.action.started += StartSecondLeftAbility;
        _secondLeftAbility.action.canceled += CancelSecondLeftAbility;

        _thirdLeftAbility.action.Enable();
        _thirdLeftAbility.action.started += StartThirdLeftAbility;
        _thirdLeftAbility.action.canceled += CancelThirdLeftAbility;
    }

    void Start()
    {
        InjectDependency();
    }

    [Button]
    private void InjectDependency()
    {
       foreach (var ability in _rightAbilities)
       {
            ability.System = this;
       }

       foreach (var ability in _leftAbilities)
       {
            ability.System = this;
       }

    }

    private void GetEchoManager(string controllerName)
    {
        if (controllerName == "Right Controller")
            _rightEchoManager = FindObjectsOfType<EchoManager>()
                                .FirstOrDefault(manager => 
                                manager.ControllerName == controllerName);
        
        else if (controllerName == "Left Controller")
            _leftEchoManager = FindObjectsOfType<EchoManager>()
                            .FirstOrDefault(manager =>
                            manager.ControllerName == controllerName);
    }

    private void StartFirstRightAbility(InputAction.CallbackContext context) => _rightAbilities[0].ActivateAbility(_rightEchoManager);
    private void CancelFirstRightAbility(InputAction.CallbackContext context) => _rightAbilities[0].CancelAbility();

    private void StartSecondRightAbility(InputAction.CallbackContext context) => _rightAbilities[1].ActivateAbility(_rightEchoManager);
    private void CancelSecondRightAbility(InputAction.CallbackContext context) => _rightAbilities[1].CancelAbility();

    private void StartThirdRightAbility(InputAction.CallbackContext context) => _rightAbilities[2].ActivateAbility(_rightEchoManager);
    private void CancelThirdRightAbility(InputAction.CallbackContext context) => _rightAbilities[2].CancelAbility();


    private void StartFirstLeftAbility(InputAction.CallbackContext context) => _leftAbilities[0].ActivateAbility(_leftEchoManager);
    private void CancelFirstLeftAbility(InputAction.CallbackContext context) => _leftAbilities[0].CancelAbility();

    private void StartSecondLeftAbility(InputAction.CallbackContext context) => _leftAbilities[0].ActivateAbility(_leftEchoManager);
    private void CancelSecondLeftAbility(InputAction.CallbackContext context) => _leftAbilities[0].CancelAbility();

    private void StartThirdLeftAbility(InputAction.CallbackContext context) => _leftAbilities[0].ActivateAbility(_leftEchoManager);
    private void CancelThirdLeftAbility(InputAction.CallbackContext context) => _leftAbilities[0].CancelAbility();


    private void OnDisable()
    {
        EventSystem.Events.OnEchoManagerStart -= GetEchoManager;

        _firstRightAbility.action.started -= StartFirstRightAbility;
        _firstRightAbility.action.canceled -= CancelFirstRightAbility;
        _firstRightAbility.action.Disable();

        _secondRightAbility.action.started -= StartSecondRightAbility;
        _secondRightAbility.action.canceled -= CancelSecondRightAbility;
        _secondRightAbility.action.Disable();

        _thirdRightAbility.action.started -= StartThirdRightAbility;
        _thirdRightAbility.action.canceled -= CancelThirdRightAbility;
        _thirdRightAbility.action.Disable();

        _firstLeftAbility.action.started -= StartFirstLeftAbility;
        _firstLeftAbility.action.canceled -= CancelFirstLeftAbility;
        _firstLeftAbility.action.Disable();

        _secondLeftAbility.action.started -= StartSecondLeftAbility;
        _secondLeftAbility.action.canceled -= CancelSecondRightAbility;
        _secondLeftAbility.action.Disable();

        _thirdLeftAbility.action.started -= StartThirdLeftAbility;
        _thirdLeftAbility.action.canceled -= CancelThirdRightAbility;
        _thirdLeftAbility.action.Disable();
    }
}
