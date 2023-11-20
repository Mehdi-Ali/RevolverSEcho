using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void OnEnable()
    {
        _firstRightAbility.action.Enable();
        _firstRightAbility.action.started += StartFirstRightAbility;
        _firstRightAbility.action.canceled += CancelFirstRightAbility;

        _firstLeftAbility.action.Enable();
        _firstLeftAbility.action.started += StartFirstLeftAbility;
        _firstLeftAbility.action.canceled += CancelFirstLeftAbility;
    }

    void Start()
    {
        InjectDependency();
        StartCoroutine(DelayedGetEchoManagers());
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

    private IEnumerator DelayedGetEchoManagers()
    {
        // !! make a better other system, probably an event that is trigered after both revolvers are instantiated 
        yield return new WaitForSeconds(1f);
        var mangers = FindObjectsOfType<EchoManager>().ToList();
        _rightEchoManager = mangers.Find(x => x.ControllerName == "Right Controller");
        _leftEchoManager = mangers.Find(x => x.ControllerName == "Left Controller");
    }


    private void StartFirstRightAbility(InputAction.CallbackContext context) => _rightAbilities[0].ActivateAbility(_rightEchoManager);
    private void CancelFirstRightAbility(InputAction.CallbackContext context) => _rightAbilities[0].CancelAbility();


    private void StartFirstLeftAbility(InputAction.CallbackContext context) => _leftAbilities[0].ActivateAbility(_leftEchoManager);
    private void CancelFirstLeftAbility(InputAction.CallbackContext context) => _leftAbilities[0].CancelAbility();

    private void OnDisable()
    {
        _firstRightAbility.action.started -= StartFirstRightAbility;
        _firstRightAbility.action.canceled -= CancelFirstRightAbility;
        _firstRightAbility.action.Disable();

        _firstLeftAbility.action.started -= StartFirstRightAbility;
        _firstLeftAbility.action.canceled -= CancelFirstRightAbility;
        _firstLeftAbility.action.Disable();
    }
}
