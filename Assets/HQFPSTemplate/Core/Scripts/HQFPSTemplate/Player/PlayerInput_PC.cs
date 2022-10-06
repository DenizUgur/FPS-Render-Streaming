using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.Controls; 

namespace HQFPSTemplate
{
	/// <summary>
	/// Simple component that handles the player input, and feeds it to the other components.
	/// </summary>
	public class PlayerInput_PC : PlayerComponent
	{
		private void Update()
		{
			if (!Player.Pause.Active && Player.ViewLocked.Is(false))
			{
				// Movement.
				var vertical = 0;
				if (Keyboard.current[Key.W].isPressed)
					vertical = 1;
				if (Keyboard.current[Key.S].isPressed)
					vertical = -1;

				var horizontal = 0;
				if (Keyboard.current[Key.D].isPressed)
					horizontal = 1;
				if (Keyboard.current[Key.A].isPressed)
					horizontal = -1;

				//Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
				Vector2 moveInput = new Vector2(horizontal, vertical);


				//Little Hack until a cleaner solution will be found
				if (Player.Aim.Active && Player.Prone.Active)
					moveInput = Vector2.zero;

				Player.MoveInput.Set(moveInput);

				// Look.
				//Player.LookInput.Set(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")));
				//Player.LookInput.Set(new Vector2(Mouse.current.position));
				//Vector2 pos = Camera.main.ScreenToViewportPoint(Mouse.current.position);
				Player.LookInput.Set(new Vector2(Mouse.current.delta.x.ReadValue(), Mouse.current.delta.y.ReadValue()));

				// Interact
				//Player.Interact.Set(Input.GetButton("Interact"));
				Player.Interact.Set(Keyboard.current[Key.E].isPressed);

				// Jump.
				//if (Input.GetButtonDown("Jump"))
				if (Keyboard.current[Key.Space].isPressed)
					Player.Jump.TryStart();

				// Run.
				//bool sprintButtonHeld = Input.GetButton("Run");
				bool sprintButtonHeld = Keyboard.current[Key.LeftShift].isPressed;

				bool canStartSprinting = Player.IsGrounded.Get() && Player.MoveInput.Get().y > 0f;

				if (!Player.Run.Active && sprintButtonHeld && canStartSprinting)
					Player.Run.TryStart();

				if (Player.Run.Active && !sprintButtonHeld)
					Player.Run.ForceStop();

				//if (Input.GetButtonDown("Crouch"))
				if (Keyboard.current[Key.C].isPressed)
				{
					if (!Player.Crouch.Active)
						Player.Crouch.TryStart();
					else
						Player.Crouch.TryStop();
				}
				//else if (Input.GetButtonDown("Prone"))
				else if (Keyboard.current[Key.Z].isPressed)
				{
					if (!Player.Prone.Active)
						Player.Prone.TryStart();
					else
						Player.Prone.TryStop();
				}

				UseEquipment();

				//Suicide (Used for testing)
				//if (Input.GetKeyDown(KeyCode.K))
				if (Keyboard.current[Key.K].isPressed)
				{
					DamageInfo damage = new DamageInfo(-1000f);
					Player.ChangeHealth.Try(damage);
				}
			}
			else
			{
				// Movement.
				Player.MoveInput.Set(Vector2Int.zero);

				// Look.
				Player.LookInput.Set(Vector2.zero);
			}

			//var scrollWheelValue = Input.GetAxisRaw("Mouse ScrollWheel");
			Vector2 scrollWheelValue = Mouse.current.scroll.ReadValue();
			Player.ScrollValue.Set(scrollWheelValue.y);
			//Player.ScrollValue.Set(scrollWheelValue);

		}	

		private void UseEquipment()
		{
			//if (Input.GetButtonDown("Drop") && !Player.EquippedItem.Is(null) && !Player.Reload.Active && !Player.Healing.Active)
			if (Keyboard.current[Key.G].isPressed && !Player.EquippedItem.Is(null) && !Player.Reload.Active && !Player.Healing.Active)
			{
				Player.DropItem.Try(Player.EquippedItem.Get());
				return;
			}

			// Change use mode
			//if (Input.GetButtonDown("ChangeUseMode"))
			if (Keyboard.current[Key.N].isPressed)
				Player.ChangeUseMode.Try();

			//bool alternateUseButtonHeld = Input.GetButton("AlternateUse");
			bool alternateUseButtonHeld = Keyboard.current[Key.N].isPressed;

			// Use item
			//if (Input.GetButtonDown("UseEquipment"))
			if (Mouse.current.leftButton.isPressed)
				Player.UseItem.Try(false, alternateUseButtonHeld ? 1 : 0);
			//else if (Input.GetButton("UseEquipment"))
			else if (Mouse.current.leftButton.isPressed)
				Player.UseItem.Try(true, alternateUseButtonHeld ? 1 : 0);

			//if (Input.GetButtonDown("ReloadEquipment"))
			if (Keyboard.current[Key.R].isPressed)
				Player.Reload.TryStart();

			// Aim
			//var aimButtonPressed = Input.GetButton("Aim");




			var aimButtonPressed = Mouse.current.rightButton.isPressed;

			if (!Player.Aim.Active && aimButtonPressed)
				Player.Aim.TryStart();
			else if (Player.Aim.Active && !aimButtonPressed)
				Player.Aim.ForceStop();

			//Heal
			//if (Input.GetButton("Heal") && !aimButtonPressed)
			if (Keyboard.current[Key.H].isPressed && !aimButtonPressed)
				Player.Healing.TryStart();
		}
	}
}
