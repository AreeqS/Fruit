# Camera Follow Setup Guide

## 🎥 **Camera Follow Script Features:**

- **Smooth following** with configurable smoothness
- **Automatic player detection** (finds objects with "Player" tag)
- **Look ahead system** that anticipates player movement
- **Boundary system** to limit camera movement
- **Configurable offset** for perfect positioning
- **Runtime methods** to change settings during gameplay

## 🚀 **Quick Setup:**

### 1. **Attach the Script**
- Select your **Main Camera** in the Hierarchy
- Drag the `CameraFollow.cs` script onto it
- OR use `Add Component` → `Scripts` → `CameraFollow`

### 2. **Basic Configuration**
- **Target**: Drag your player GameObject here (optional - auto-detects if tagged)
- **Offset**: Set camera position relative to player (default: 0, 2, -10)
- **Smooth Speed**: How smoothly camera follows (5 = smooth, 10 = very smooth)

### 3. **Player Tag Setup** (Recommended)
- Select your player GameObject
- In Inspector, set **Tag** to "Player"
- This allows the camera to auto-detect your player

## ⚙️ **Advanced Settings:**

### **Follow Settings:**
- **Follow X**: Enable/disable horizontal following
- **Follow Y**: Enable/disable vertical following

### **Boundaries:**
- **Use Boundaries**: Enable to limit camera movement
- **Min/Max X/Y**: Set camera movement limits
- **Visual Help**: Yellow wire cube shows boundaries in Scene view

### **Look Ahead:**
- **Use Look Ahead**: Camera anticipates player movement direction
- **Look Ahead Distance**: How far ahead to look (2 = 2 units)
- **Look Ahead Smooth Speed**: How smoothly to transition

## 🎯 **Recommended Settings:**

### **For 2D Platformer:**
```
Target: [Your Player GameObject]
Offset: (0, 2, -10)
Smooth Speed: 5
Follow X: ✓
Follow Y: ✓
Use Boundaries: ✓
Min X: -20, Max X: 20
Min Y: -10, Max Y: 10
Use Look Ahead: ✓
Look Ahead Distance: 2
```

### **For Top-Down Game:**
```
Target: [Your Player GameObject]
Offset: (0, 0, -10)
Smooth Speed: 8
Follow X: ✓
Follow Y: ✓
Use Boundaries: ✗
Use Look Ahead: ✗
```

## 🔧 **Troubleshooting:**

### **Camera not following?**
- Check if target is assigned
- Verify player has "Player" tag
- Check Console for error messages

### **Camera too jittery?**
- Lower the **Smooth Speed** value
- Try values between 3-7

### **Camera too slow?**
- Increase the **Smooth Speed** value
- Try values between 8-15

### **Camera going out of bounds?**
- Enable **Use Boundaries**
- Adjust Min/Max X and Y values
- Check the yellow wire cube in Scene view

## 🎮 **Runtime Methods:**

You can call these methods from other scripts:

```csharp
// Get camera reference
CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();

// Change target
cameraFollow.SetTarget(newPlayer.transform);

// Change offset
cameraFollow.SetOffset(new Vector3(0, 5, -15));

// Enable boundaries
cameraFollow.SetBoundaries(true, -30, 30, -15, 15);
```

## 📱 **Mobile/Performance Tips:**

- **Smooth Speed**: Use lower values (3-5) for better performance
- **Look Ahead**: Disable on mobile for better performance
- **Boundaries**: Enable to prevent unnecessary calculations

## 🎨 **Visual Debugging:**

- **Cyan sphere**: Shows where camera wants to be
- **Yellow wire cube**: Shows camera boundaries (when enabled)
- **Cyan line**: Shows offset from player to camera target

The camera will now smoothly follow your player with professional-looking movement!

