//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Sources/Services/Input/InputActionsSheme.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Services.Input
{
    public partial class @InputActionsSheme: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputActionsSheme()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActionsSheme"",
    ""maps"": [
        {
            ""name"": ""GameplayInput"",
            ""id"": ""fdde530e-b972-49b9-83b5-a918106ee15e"",
            ""actions"": [
                {
                    ""name"": ""HandlePressedMove"",
                    ""type"": ""Value"",
                    ""id"": ""9a464921-041a-40bd-84e8-04316e79395b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""HandleMove"",
                    ""type"": ""Value"",
                    ""id"": ""7945845f-6bc8-4dc2-9377-fad9ec129c5d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Press"",
                    ""type"": ""Value"",
                    ""id"": ""f0b0b76c-cde2-499f-8de8-1a59a27838df"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""One Modifier"",
                    ""id"": ""f78891ff-2cd0-42c4-a1b8-aa0b8fca32eb"",
                    ""path"": ""OneModifier"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandlePressedMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""4c9f7686-c714-4cc0-8361-ae7b2e50d926"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandlePressedMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Binding"",
                    ""id"": ""b7e3a017-08a5-43ad-a566-39a197db1db4"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandlePressedMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5138a63f-e945-40c7-8290-f3d252e660cf"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandleMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""One Modifier"",
                    ""id"": ""08245fd8-d89d-4d88-b20f-1190ea8bb3a9"",
                    ""path"": ""OneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Press"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""9cbf0a8f-3e06-41ba-98b3-a0df25f0af76"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""ce8b8914-5eb4-4483-86c9-c6e287ad08fc"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""GameplayWindowInput"",
            ""id"": ""74df02b6-d5d6-409a-b9b2-1acdbf7f4155"",
            ""actions"": [
                {
                    ""name"": ""UndoButtonPressed"",
                    ""type"": ""Button"",
                    ""id"": ""94887cc2-64aa-4951-914e-8d0a283f3301"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RemoveBuildingButtonPressed"",
                    ""type"": ""Button"",
                    ""id"": ""dd8cc7a5-5049-42ab-9cc2-5b2dcf306e5c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ReplaceBuildingButtonPressed"",
                    ""type"": ""Button"",
                    ""id"": ""df085374-e02b-4a7b-8203-3e68caa7ea7e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b6237c84-4f2b-4dca-ae21-3e9a67a57033"",
                    ""path"": ""<Touchscreen>/touch0/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UndoButtonPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be247254-0c51-43e8-96ff-92465681d723"",
                    ""path"": ""<Touchscreen>/touch1/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RemoveBuildingButtonPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""76223388-43cc-4041-a3da-2592ac1b396e"",
                    ""path"": ""<Touchscreen>/touch2/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ReplaceBuildingButtonPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // GameplayInput
            m_GameplayInput = asset.FindActionMap("GameplayInput", throwIfNotFound: true);
            m_GameplayInput_HandlePressedMove = m_GameplayInput.FindAction("HandlePressedMove", throwIfNotFound: true);
            m_GameplayInput_HandleMove = m_GameplayInput.FindAction("HandleMove", throwIfNotFound: true);
            m_GameplayInput_Press = m_GameplayInput.FindAction("Press", throwIfNotFound: true);
            // GameplayWindowInput
            m_GameplayWindowInput = asset.FindActionMap("GameplayWindowInput", throwIfNotFound: true);
            m_GameplayWindowInput_UndoButtonPressed = m_GameplayWindowInput.FindAction("UndoButtonPressed", throwIfNotFound: true);
            m_GameplayWindowInput_RemoveBuildingButtonPressed = m_GameplayWindowInput.FindAction("RemoveBuildingButtonPressed", throwIfNotFound: true);
            m_GameplayWindowInput_ReplaceBuildingButtonPressed = m_GameplayWindowInput.FindAction("ReplaceBuildingButtonPressed", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // GameplayInput
        private readonly InputActionMap m_GameplayInput;
        private List<IGameplayInputActions> m_GameplayInputActionsCallbackInterfaces = new List<IGameplayInputActions>();
        private readonly InputAction m_GameplayInput_HandlePressedMove;
        private readonly InputAction m_GameplayInput_HandleMove;
        private readonly InputAction m_GameplayInput_Press;
        public struct GameplayInputActions
        {
            private @InputActionsSheme m_Wrapper;
            public GameplayInputActions(@InputActionsSheme wrapper) { m_Wrapper = wrapper; }
            public InputAction @HandlePressedMove => m_Wrapper.m_GameplayInput_HandlePressedMove;
            public InputAction @HandleMove => m_Wrapper.m_GameplayInput_HandleMove;
            public InputAction @Press => m_Wrapper.m_GameplayInput_Press;
            public InputActionMap Get() { return m_Wrapper.m_GameplayInput; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GameplayInputActions set) { return set.Get(); }
            public void AddCallbacks(IGameplayInputActions instance)
            {
                if (instance == null || m_Wrapper.m_GameplayInputActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_GameplayInputActionsCallbackInterfaces.Add(instance);
                @HandlePressedMove.started += instance.OnHandlePressedMove;
                @HandlePressedMove.performed += instance.OnHandlePressedMove;
                @HandlePressedMove.canceled += instance.OnHandlePressedMove;
                @HandleMove.started += instance.OnHandleMove;
                @HandleMove.performed += instance.OnHandleMove;
                @HandleMove.canceled += instance.OnHandleMove;
                @Press.started += instance.OnPress;
                @Press.performed += instance.OnPress;
                @Press.canceled += instance.OnPress;
            }

            private void UnregisterCallbacks(IGameplayInputActions instance)
            {
                @HandlePressedMove.started -= instance.OnHandlePressedMove;
                @HandlePressedMove.performed -= instance.OnHandlePressedMove;
                @HandlePressedMove.canceled -= instance.OnHandlePressedMove;
                @HandleMove.started -= instance.OnHandleMove;
                @HandleMove.performed -= instance.OnHandleMove;
                @HandleMove.canceled -= instance.OnHandleMove;
                @Press.started -= instance.OnPress;
                @Press.performed -= instance.OnPress;
                @Press.canceled -= instance.OnPress;
            }

            public void RemoveCallbacks(IGameplayInputActions instance)
            {
                if (m_Wrapper.m_GameplayInputActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IGameplayInputActions instance)
            {
                foreach (var item in m_Wrapper.m_GameplayInputActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_GameplayInputActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public GameplayInputActions @GameplayInput => new GameplayInputActions(this);

        // GameplayWindowInput
        private readonly InputActionMap m_GameplayWindowInput;
        private List<IGameplayWindowInputActions> m_GameplayWindowInputActionsCallbackInterfaces = new List<IGameplayWindowInputActions>();
        private readonly InputAction m_GameplayWindowInput_UndoButtonPressed;
        private readonly InputAction m_GameplayWindowInput_RemoveBuildingButtonPressed;
        private readonly InputAction m_GameplayWindowInput_ReplaceBuildingButtonPressed;
        public struct GameplayWindowInputActions
        {
            private @InputActionsSheme m_Wrapper;
            public GameplayWindowInputActions(@InputActionsSheme wrapper) { m_Wrapper = wrapper; }
            public InputAction @UndoButtonPressed => m_Wrapper.m_GameplayWindowInput_UndoButtonPressed;
            public InputAction @RemoveBuildingButtonPressed => m_Wrapper.m_GameplayWindowInput_RemoveBuildingButtonPressed;
            public InputAction @ReplaceBuildingButtonPressed => m_Wrapper.m_GameplayWindowInput_ReplaceBuildingButtonPressed;
            public InputActionMap Get() { return m_Wrapper.m_GameplayWindowInput; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GameplayWindowInputActions set) { return set.Get(); }
            public void AddCallbacks(IGameplayWindowInputActions instance)
            {
                if (instance == null || m_Wrapper.m_GameplayWindowInputActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_GameplayWindowInputActionsCallbackInterfaces.Add(instance);
                @UndoButtonPressed.started += instance.OnUndoButtonPressed;
                @UndoButtonPressed.performed += instance.OnUndoButtonPressed;
                @UndoButtonPressed.canceled += instance.OnUndoButtonPressed;
                @RemoveBuildingButtonPressed.started += instance.OnRemoveBuildingButtonPressed;
                @RemoveBuildingButtonPressed.performed += instance.OnRemoveBuildingButtonPressed;
                @RemoveBuildingButtonPressed.canceled += instance.OnRemoveBuildingButtonPressed;
                @ReplaceBuildingButtonPressed.started += instance.OnReplaceBuildingButtonPressed;
                @ReplaceBuildingButtonPressed.performed += instance.OnReplaceBuildingButtonPressed;
                @ReplaceBuildingButtonPressed.canceled += instance.OnReplaceBuildingButtonPressed;
            }

            private void UnregisterCallbacks(IGameplayWindowInputActions instance)
            {
                @UndoButtonPressed.started -= instance.OnUndoButtonPressed;
                @UndoButtonPressed.performed -= instance.OnUndoButtonPressed;
                @UndoButtonPressed.canceled -= instance.OnUndoButtonPressed;
                @RemoveBuildingButtonPressed.started -= instance.OnRemoveBuildingButtonPressed;
                @RemoveBuildingButtonPressed.performed -= instance.OnRemoveBuildingButtonPressed;
                @RemoveBuildingButtonPressed.canceled -= instance.OnRemoveBuildingButtonPressed;
                @ReplaceBuildingButtonPressed.started -= instance.OnReplaceBuildingButtonPressed;
                @ReplaceBuildingButtonPressed.performed -= instance.OnReplaceBuildingButtonPressed;
                @ReplaceBuildingButtonPressed.canceled -= instance.OnReplaceBuildingButtonPressed;
            }

            public void RemoveCallbacks(IGameplayWindowInputActions instance)
            {
                if (m_Wrapper.m_GameplayWindowInputActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IGameplayWindowInputActions instance)
            {
                foreach (var item in m_Wrapper.m_GameplayWindowInputActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_GameplayWindowInputActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public GameplayWindowInputActions @GameplayWindowInput => new GameplayWindowInputActions(this);
        public interface IGameplayInputActions
        {
            void OnHandlePressedMove(InputAction.CallbackContext context);
            void OnHandleMove(InputAction.CallbackContext context);
            void OnPress(InputAction.CallbackContext context);
        }
        public interface IGameplayWindowInputActions
        {
            void OnUndoButtonPressed(InputAction.CallbackContext context);
            void OnRemoveBuildingButtonPressed(InputAction.CallbackContext context);
            void OnReplaceBuildingButtonPressed(InputAction.CallbackContext context);
        }
    }
}
