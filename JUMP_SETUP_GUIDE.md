# Jump and Ground Detection Setup Guide

## Issues Fixed:
1. ✅ **Jumping after one jump**: Fixed jump count reset logic
2. ✅ **IsGrounded function not working**: Added proper ground check setup and null safety
3. ✅ **Ground detection**: Auto-creates ground check object if not assigned

## Setup Steps:

### 1. Player GameObject Setup
- Make sure your player has a `Rigidbody2D` component
- Set `Rigidbody2D` → `Constraints` → Freeze Rotation Z to prevent spinning
- Set `Rigidbody2D` → `Gravity Scale` to 1 (or adjust as needed)

### 2. Ground Layer Setup
- In Unity, go to `Edit` → `Project Settings` → `Tags and Layers`
- Create a new layer called "Ground" (or use existing "Default" layer)
- Assign this layer to all your ground/platform objects

### 3. Script Configuration
- Select your player GameObject
- In the Inspector, find the `PlayerController2D` script
- Set `Ground Layer` to the layer you created (or leave as "Default" if using default layer)
- Adjust `Ground Check Radius` if needed (default 0.2f should work)

### 4. Ground Check Object (Auto-created)
- The script automatically creates a "GroundCheck" child object
- It's positioned at the bottom of your player (local position 0, -0.5f, 0)
- You can manually adjust this position in the Inspector if needed

### 5. Ground Objects
- Make sure all ground/platform objects have:
  - A Collider2D component (Box Collider 2D, Circle Collider 2D, etc.)
  - The correct layer assigned (same as `Ground Layer` in the script)

## Troubleshooting:

### Still can't jump?
- Check the Console for error messages
- Verify the player has a `Rigidbody2D` component
- Make sure ground objects have colliders and the right layer

### Ground detection not working?
- Look for the yellow wire sphere in Scene view (select player and enable Gizmos)
- Adjust `Ground Check Radius` if the sphere doesn't touch the ground
- Verify ground objects have the correct layer assigned

### Jump feels wrong?
- Adjust `Jump Force` in the Inspector
- Modify `Fall Multiplier` and `Low Jump Multiplier` for better feel
- Change `Max Jumps` if you want single or triple jump instead of double

## Debug Info:
The script shows debug information in the Inspector:
- `Current Jumps Remaining`: Shows how many jumps you have left
- `Is Currently Grounded`: Shows if the player is touching the ground

## Recommended Settings:
- `Move Speed`: 5
- `Jump Force`: 10
- `Ground Check Radius`: 0.2
- `Max Jumps`: 2 (for double jump)
- `Coyote Time`: 0.1 (for forgiving jump timing)

