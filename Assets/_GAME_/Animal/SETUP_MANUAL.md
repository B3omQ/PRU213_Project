# Hướng dẫn Setup Thủ công Animal System - Chi tiết

## Tổng quan hệ thống

Hệ thống Animal bao gồm 8 scripts chính (setup thủ công):
- **Animal.cs** - Script cơ bản cho từng con vật
- **AnimalPool.cs** - Object pooling system
- **AnimalSpawner.cs** - Hệ thống spawn animals
- **AnimalManager.cs** - Quản lý toàn bộ hệ thống
- **AnimalVariants.cs** - Quản lý biến thể (màu sắc) - Optional
- **AnimalSpawnConfig.cs** - Cấu hình spawn nâng cao - Optional
- **AnimalBehavior.cs** - AI behavior cho animals - Optional
- **AnimalInteraction.cs** - Tương tác với player - Optional

**Lưu ý:** Scripts có gắn "Optional" có thể bỏ qua nếu chỉ cần hệ thống cơ bản.

**Lưu ý Unity Version:** Hướng dẫn này sử dụng Unity 2022.3+ với Rigidbody2D properties:
- **Linear Damping** (thay vì Drag)
- **Angular Damping** (thay vì Angular Drag)
- **linearVelocity** (thay vì velocity)

## Setup cơ bản vs nâng cao:

### 🟢 **Setup cơ bản (Bắt buộc):**
- **Animal.cs** - Script chính cho từng con vật
- **AnimalPool.cs** - Object pooling system
- **AnimalSpawner.cs** - Hệ thống spawn animals
- **AnimalManager.cs** - Quản lý toàn bộ hệ thống

### 🟡 **Setup nâng cao (Optional):**
- **AnimalVariants.cs** - Quản lý biến thể (màu sắc)
- **AnimalSpawnConfig.cs** - Cấu hình spawn nâng cao
- **AnimalBehavior.cs** - AI behavior cho animals
- **AnimalInteraction.cs** - Tương tác với player
- **AnimalAreaManager.cs** - Quản lý vùng di chuyển và drop items
- **AnimalDropItem.cs** - Script cho drop items

### 📋 **Hướng dẫn setup:**
1. **Bắt đầu với setup cơ bản** (Bước 1-8)
2. **Test hệ thống cơ bản** trước
3. **Thêm features nâng cao** nếu cần
4. **Mỗi feature nâng cao** có thể thêm riêng lẻ

## Bước 1: Tạo AnimalPool GameObject

### 1.1 Tạo GameObject:
1. **Right-click trong Hierarchy**
2. **Chọn Create Empty**
3. **Đặt tên: `AnimalPool`**
4. **Đặt vị trí: (0, 0, 0)**

### 1.2 Thêm AnimalPool script:
1. **Select AnimalPool GameObject**
2. **Trong Inspector, click Add Component**
3. **Tìm và chọn: Scripts → Animal → AnimalPool**
4. **Kiểm tra script đã được thêm**

### 1.3 Setup AnimalPool:
1. **Pool Size: 50** (số lượng objects trong pool)
2. **Tạo child GameObject:**
   - Right-click trên AnimalPool
   - Create Empty
   - Đặt tên: `PoolParent`
3. **Gán PoolParent:**
   - Select AnimalPool
   - Trong AnimalPool script, kéo PoolParent vào field "Pool Parent"
4. **Kiểm tra:**
   - Pool Parent field có hiển thị PoolParent
   - Pool Size = 50

## Bước 1.5: Tạo AnimalAreaManager GameObject (Cho tính năng vùng di chuyển)

### 1.5.1 Tạo GameObject:
1. **Right-click trong Hierarchy**
2. **Chọn Create Empty**
3. **Đặt tên: `AnimalAreaManager`**
4. **Đặt vị trí: (0, 0, 0)**

### 1.5.2 Thêm AnimalAreaManager script:
1. **Select AnimalAreaManager GameObject**
2. **Trong Inspector, click Add Component**
3. **Tìm và chọn: Scripts → Animal → AnimalAreaManager**
4. **Kiểm tra script đã được thêm**

### 1.5.3 Setup AnimalAreaManager:
1. **Area Center: gán AnimalSpawner GameObject** (hoặc tạo Empty GameObject làm center)
2. **Area Radius: 15** (bán kính vùng di chuyển)
3. **Show Area Gizmos: ✓** (hiển thị vùng trong Scene view)
4. **Area Color: Green** (màu vùng)
5. **Stay In Area: ✓** (animals không ra khỏi vùng)
6. **Change Direction Interval: 3** (giây thay đổi hướng)

## Bước 2: Tạo AnimalManager GameObject

### 2.1 Tạo GameObject:
1. **Right-click trong Hierarchy**
2. **Chọn Create Empty**
3. **Đặt tên: `AnimalManager`**
4. **Đặt vị trí: (0, 0, 0)**

### 2.2 Thêm AnimalManager script:
1. **Select AnimalManager GameObject**
2. **Trong Inspector, click Add Component**
3. **Tìm và chọn: Scripts → Animal → AnimalManager**
4. **Kiểm tra script đã được thêm**

### 2.3 Setup AnimalManager:
1. **Enable Show Debug Info: ✓** (để hiển thị debug UI)
2. **Global Spawning Enabled: ✓** (để tự động spawn)
3. **Global Move Speed: 2** (tốc độ di chuyển mặc định)
4. **Global Health: 100** (máu mặc định)
5. **Animals Can Move: ✓** (cho phép di chuyển)

## Bước 3: Tạo AnimalSpawner GameObject

### 3.1 Tạo GameObject:
1. **Right-click trong Hierarchy**
2. **Chọn Create Empty**
3. **Đặt tên: `AnimalSpawner`**
4. **Đặt vị trí: (0, 0, 0)**

### 3.2 Thêm AnimalSpawner script:
1. **Select AnimalSpawner GameObject**
2. **Trong Inspector, click Add Component**
3. **Tìm và chọn: Scripts → Animal → AnimalSpawner**
4. **Kiểm tra script đã được thêm**

### 3.3 Setup AnimalSpawner:
1. **Spawn Interval: 2** (giây giữa các lần spawn)
2. **Max Animals: 20** (số lượng tối đa)
3. **Spawn Radius: 10** (bán kính spawn area)
4. **Spawn On Start: ✓** (tự động spawn khi bắt đầu)
5. **Continuous Spawning: ✓** (spawn liên tục)
6. **Randomize Animal Type: ✓** (spawn ngẫu nhiên)
7. **Spawn Center: gán chính GameObject này**
8. **Min Distance From Player: 5** (khoảng cách tối thiểu từ player)

## Bước 4: Import Sprites

### 4.1 Copy sprites từ external folder:
1. **Mở File Explorer**
2. **Đi đến: `C:\Users\Admin\Downloads\PRU\Animal\Animals`**
3. **Copy tất cả folder animals:**
   - Cat, CatCyclop, Chicken, Cow, Cub, Dog, Dog2, Donkey, Fish, Frog, Horse, Lion, Lioness, Monkey, Parrot, Pig, Racoon
4. **Paste vào: `Assets/_GAME_/Animal/Visuals/Spirite/Animals`**
5. **Copy folder Farm Animals vào: `Assets/_GAME_/Animal/Visuals/Spirite/Farm Animals`**

### 4.2 Import vào Unity:
1. **Refresh Asset Database:**
   - Nhấn Ctrl+R
   - Hoặc click Assets → Refresh
2. **Kiểm tra sprites đã import:**
   - Mở Project window
   - Đi đến Assets/_GAME_/Animal/Visuals/Spirite
   - Kiểm tra có đủ folders không

### 4.3 Setup Sprite Import Settings:
1. **Chọn một sprite bất kỳ (ví dụ: Cat/SpriteSheet.png)**
2. **Trong Inspector:**
   - Sprite Mode: Multiple
   - Pixels Per Unit: 32
   - Filter Mode: Point (no filter)
   - Compression: None
3. **Click Sprite Editor**
4. **Trong Sprite Editor:**
   - Click Slice
   - Type: Automatic
   - Click Apply
5. **Lặp lại cho tất cả sprites cần thiết**

## Bước 5: Tạo Animal Prefabs

### 5.1 Tạo Cat prefab (ví dụ chi tiết):

#### 5.1.1 Tạo GameObject:
1. **Right-click trong Hierarchy**
2. **Chọn Create Empty**
3. **Đặt tên: `Cat`**
4. **Đặt vị trí: (0, 0, 0)**

#### 5.1.2 Thêm SpriteRenderer:
1. **Select Cat GameObject**
2. **Add Component → Sprite Renderer**
3. **Trong Sprite field:**
   - Click circle icon
   - Tìm và chọn Cat sprite từ Assets/_GAME_/Animal/Visuals/Spirite/Animals/Cat
4. **Sorting Order: 1** (để hiển thị trên background)

#### 5.1.3 Thêm Collider:
1. **Add Component → Box Collider 2D**
2. **Is Trigger: ✓** (để detect collision)
3. **Size: (1, 1)** (kích thước collider)

#### 5.1.4 Thêm Rigidbody2D:
1. **Add Component → Rigidbody 2D**
2. **Gravity Scale: 0** (không bị rơi)
3. **Linear Damping: 2** (ma sát di chuyển)
4. **Angular Damping: 5** (ma sát xoay)

#### 5.1.5 Thêm Animal script:
1. **Add Component → Scripts → Animal → Animal**
2. **Animal Type: Cat**
3. **Move Speed: 2**
4. **Health: 100**
5. **Max Health: 100**
6. **Has Lying Animation: ✓** (cho phép animation nằm)
7. **Lying Threshold: 0.1** (tốc độ dưới ngưỡng này sẽ nằm)
8. **Lying Delay: 2** (giây chờ trước khi nằm)

#### 5.1.6 Thêm Animator (Cho animation nằm/xuống) - CHI TIẾT:

##### Bước 1: Thêm Animator Component
1. **Select Cat GameObject**
2. **Add Component → Animator**
3. **Kiểm tra Animator component đã được thêm**

##### Bước 2: Tạo Animator Controller
1. **Right-click trong Project window**
2. **Create → Animator Controller**
3. **Đặt tên: `CatAnimatorController`**
4. **Lưu tại: `Assets/_GAME_/Animal/Visuals/Prefab/Animators/`**

##### Bước 3: Gán Controller
1. **Select Cat GameObject**
2. **Trong Animator component:**
   - **Controller:** Kéo CatAnimatorController vào field này
3. **Kiểm tra:** Controller field hiển thị CatAnimatorController

##### Bước 4: Mở Animator Window
1. **Window → Animation → Animator**
2. **Hoặc double-click CatAnimatorController**
3. **Animator window sẽ mở ra**

##### Bước 5: Tạo Animation Clips (CHI TIẾT)
1. **Tạo folder Animations:**
   - Right-click trong Project window
   - Create → Folder
   - Đặt tên: `Animations`
   - Đặt tại: `Assets/_GAME_/Animal/Visuals/Prefab/`

2. **Tạo Animation Clip cho Idle:**
   - Right-click trong Animations folder
   - Create → Animation
   - Đặt tên: `Cat_Idle`
   - **Lưu ý:** File sẽ có extension .anim

3. **Tạo Animation Clip cho Walking:**
   - Right-click trong Animations folder
   - Create → Animation
   - Đặt tên: `Cat_Walking`

4. **Tạo Animation Clip cho Lying:**
   - Right-click trong Animations folder
   - Create → Animation
   - Đặt tên: `Cat_Lying`

5. **Kiểm tra:** Có 3 file .anim trong Animations folder

##### Bước 6: Setup Animation Clips (CHI TIẾT)

###### 6.1 Setup Cat_Idle.anim:
1. **QUAN TRỌNG: Select Cat GameObject trong Hierarchy trước**
2. **Double-click Cat_Idle.anim**
3. **Animation window mở ra**
4. **Trong Animation window:**
   - **Length: 1** (1 giây)
   - **Loop Time: ✓** (lặp lại)
5. **Tạo keyframe:**
   - **Click Record button** (hình tròn đỏ) - BÂY GIỜ SẼ ẤN ĐƯỢC
   - **BƯỚC TIẾP THEO:**
     1. **Click "Add Property" button** (nút xám lớn ở giữa)
     2. **Menu hiện ra, chọn:**
        - **Sprite Renderer** (expand)
        - **Sprite** (check box)
     3. **Sprite Renderer.Sprite xuất hiện trong animation track**
     4. **Tại keyframe 0:** 
        - **Click vào Sprite field** trong Inspector
        - **Chọn sprite đứng yên của Cat** từ Project window
     5. **Click Record button** để tắt (nút đỏ)
6. **Kiểm tra:** Animation có 1 keyframe tại 0:00

###### 6.2 Setup Cat_Walking.anim:
1. **QUAN TRỌNG: Select Cat GameObject trong Hierarchy trước**
2. **Double-click Cat_Walking.anim**
3. **Trong Animation window:**
   - **Length: 1** (1 giây)
   - **Loop Time: ✓** (lặp lại)
4. **Tạo keyframes:**
   - **Click Record button** (hình tròn đỏ) - BÂY GIỜ SẼ ẤN ĐƯỢC
   - **BƯỚC TIẾP THEO:**
     1. **Click "Add Property" button** (nút xám lớn)
     2. **Chọn "Sprite Renderer" → "Sprite"**
     3. **Tại keyframe 0:** Gán sprite đi bộ 1 của Cat
     4. **Tại keyframe 0.5:** Gán sprite đi bộ 2 của Cat
     5. **Tại keyframe 1:** Gán sprite đi bộ 1 của Cat (lặp lại)
     6. **Click Record button** để tắt
5. **Kiểm tra:** Animation có 3 keyframes (0, 0.5, 1)

###### 6.3 Setup Cat_Lying.anim:
1. **QUAN TRỌNG: Select Cat GameObject trong Hierarchy trước**
2. **Double-click Cat_Lying.anim**
3. **Trong Animation window:**
   - **Length: 2** (2 giây)
   - **Loop Time: ✗** (không lặp)
4. **Tạo keyframe:**
   - **Click Record button** (hình tròn đỏ) - BÂY GIỜ SẼ ẤN ĐƯỢC
   - **BƯỚC TIẾP THEO:**
     1. **Click "Add Property" button** (nút xám lớn)
     2. **Chọn "Sprite Renderer" → "Sprite"**
     3. **Tại keyframe 0:** Gán sprite nằm của Cat
     4. **Click Record button** để tắt
5. **Kiểm tra:** Animation có 1 keyframe tại 0:00

###### 6.4 Lưu ý về Sprites:
- **Idle:** Dùng 1 sprite đứng yên
- **Walking:** Dùng 2-3 sprites đi bộ (chân trước, chân sau)
- **Lying:** Dùng 1 sprite nằm xuống
- **Nếu không có sprites riêng:** Dùng cùng 1 sprite cho tất cả

##### Bước 7: Tạo Animation States trong Animator
1. **Trong Animator window:**
2. **Right-click → Create State → Empty**
3. **Tạo 3 states:**
   - **Idle** (màu xanh lá)
   - **Walking** (màu xanh dương)
   - **Lying** (màu đỏ)
4. **Gán Animation Clips:**
   - **Idle state:** Kéo Cat_Idle.anim vào
   - **Walking state:** Kéo Cat_Walking.anim vào
   - **Lying state:** Kéo Cat_Lying.anim vào

##### Bước 8: Setup Animator Parameters
1. **Trong Animator window, click Parameters tab**
2. **Click + → Bool**
3. **Tạo 2 parameters:**
   - **IsMoving** (Bool)
   - **IsLying** (Bool)
4. **Kiểm tra:** Parameters list hiển thị 2 parameters

##### Bước 9: Setup Transitions (CHI TIẾT)
1. **Right-click Idle state → Make Transition → Walking**
2. **Click transition arrow:**
   - **Has Exit Time: ✗**
   - **Transition Duration: 0.2**
   - **Conditions: IsMoving = true**
3. **Right-click Walking state → Make Transition → Idle**
4. **Click transition arrow:**
   - **Has Exit Time: ✗**
   - **Transition Duration: 0.2**
   - **Conditions: IsMoving = false**
5. **Right-click Idle state → Make Transition → Lying**
6. **Click transition arrow:**
   - **Has Exit Time: ✗**
   - **Transition Duration: 0.3**
   - **Conditions: IsLying = true**
7. **Right-click Lying state → Make Transition → Idle**
8. **Click transition arrow:**
   - **Has Exit Time: ✗**
   - **Transition Duration: 0.3**
   - **Conditions: IsLying = false**
9. **Right-click Walking state → Make Transition → Lying**
10. **Click transition arrow:**
    - **Has Exit Time: ✗**
    - **Transition Duration: 0.3**
    - **Conditions: IsLying = true**
11. **Right-click Lying state → Make Transition → Walking**
12. **Click transition arrow:**
    - **Has Exit Time: ✗**
    - **Transition Duration: 0.3**
    - **Conditions: IsMoving = true**

##### Bước 10: Set Default State
1. **Right-click Idle state → Set as Layer Default State**
2. **Idle state sẽ có màu cam** (default state)

##### Bước 11: Test Animation
1. **Play scene**
2. **Quan sát Cat di chuyển:**
   - **Khi di chuyển:** Walking animation
   - **Khi dừng:** Idle animation
   - **Khi nằm:** Lying animation
3. **Kiểm tra Animator window:**
   - **States chuyển đổi** theo parameters
   - **Transitions mượt mà**

##### Bước 12: Tạo Prefab
1. **Drag Cat GameObject vào Prefab folder**
2. **Xóa Cat GameObject khỏi scene**
3. **Kiểm tra prefab có Animator component**

##### Lưu ý quan trọng:
- **Chỉ tạo Animator cho 5 loài:** Cow, Donkey, Horse, Lion, Lioness
- **Các loài khác không cần Animator**
- **Animation clips phải được tạo trước**
- **Test kỹ transitions trước khi tạo prefab**

##### Troubleshooting Animator:

###### Vấn đề 1: Animation không chạy
- **Nguyên nhân:** Animator Controller chưa được gán
- **Giải pháp:** Kiểm tra Controller field trong Animator component

###### Vấn đề 2: Transitions không hoạt động
- **Nguyên nhân:** Parameters chưa được setup đúng
- **Giải pháp:** Kiểm tra IsMoving và IsLying parameters

###### Vấn đề 3: Animation bị giật
- **Nguyên nhân:** Transition Duration quá ngắn
- **Giải pháp:** Tăng Transition Duration lên 0.3-0.5

###### Vấn đề 4: States không chuyển đổi
- **Nguyên nhân:** Conditions chưa đúng
- **Giải pháp:** Kiểm tra IsMoving = true/false, IsLying = true/false

###### Vấn đề 5: Animation clips không hiển thị
- **Nguyên nhân:** Animation clips chưa được gán vào states
- **Giải pháp:** Kéo animation clips vào Motion field của states

###### Vấn đề 6: Loop không hoạt động
- **Nguyên nhân:** Loop Time chưa được bật
- **Giải pháp:** Bật Loop Time trong Animation window

###### Vấn đề 7: Record button không ấn được (GRAYED OUT)
- **Nguyên nhân:** Chưa select GameObject trong Hierarchy
- **Giải pháp:** 
  1. **Select Cat GameObject trong Hierarchy trước**
  2. **Sau đó mở Animation window**
  3. **Record button sẽ sáng lên và ấn được**
- **Lưu ý:** Phải select GameObject trước khi record animation

##### Test Animation:
1. **Play scene**
2. **Quan sát Cat trong Scene view:**
   - **Di chuyển:** Walking animation
   - **Dừng:** Idle animation
   - **Nằm:** Lying animation
3. **Kiểm tra Animator window:**
   - **States chuyển đổi** theo parameters
   - **Transitions mượt mà**
   - **Không có lỗi** (màu đỏ)

#### 5.1.7 Thêm AnimalBehavior script (Optional):
1. **Add Component → Scripts → Animal → AnimalBehavior**
2. **Animal Type: Cat**
3. **Behavior Type: Curious**
4. **Move Speed: 2**
5. **Can Move: ✓**
6. **Lưu ý:** Script này là optional, có thể bỏ qua nếu chỉ cần di chuyển cơ bản

#### 5.1.8 Thêm AnimalInteraction script (Optional):
1. **Add Component → Scripts → Animal → AnimalInteraction**
2. **Can Be Fed: ✓**
3. **Can Be Tamed: ✓**
4. **Can Be Petted: ✓**
5. **Lưu ý:** Script này là optional, có thể bỏ qua nếu không cần tương tác với player

#### 5.1.9 Tạo prefab:
1. **Drag Cat GameObject từ Hierarchy vào `Assets/_GAME_/Animal/Visuals/Prefab`**
2. **Xóa Cat GameObject khỏi scene**
3. **Kiểm tra prefab đã được tạo trong Project window**

### 5.2 Tạo các prefabs khác:

#### 5.2.1 Chicken prefab (Farm Animal - Passive):
- **Tên: `Chicken`**
- **Sprite: Chicken/SpriteSheetWhite** (màu trắng cơ bản)
- **Animal Type: Chicken**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 0.8, 0.8)
  - Rigidbody2D ✓ (Linear Damping: 3, Angular Damping: 5)
  - Animal script ✓ (Move Speed: 1.5, Health: 50)
- **Optional components:**
  - AnimalBehavior (Passive) - Optional
  - AnimalInteraction (Can Be Fed, Can Be Tamed) - Optional

#### 5.2.2 Cow prefab (Farm Animal - Large - CÓ NẰM):
- **Tên: `Cow`**
- **Sprite: Cow/SpriteSheetWhite**
- **Animal Type: Cow**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 1.2, 1.2)
  - Rigidbody2D ✓ (Linear Damping: 4, Angular Damping: 6)
  - Animal script ✓ (Move Speed: 1, Health: 150, Has Lying Animation: ✓, Lying Delay: 4)
  - Animator ✓ (với Lying animation)
- **Optional components:**
  - AnimalBehavior (Passive) - Optional
  - AnimalInteraction (Can Be Fed, Can Be Tamed, Can Be Petted) - Optional

#### 5.2.3 Dog prefab (Pet - Friendly - KHÔNG NẰM):
- **Tên: `Dog`**
- **Sprite: Dog/SpriteSheet**
- **Animal Type: Dog**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 1, 1)
  - Rigidbody2D ✓ (Linear Damping: 2, Angular Damping: 4)
  - Animal script ✓ (Move Speed: 2.5, Health: 80, Has Lying Animation: ✗)
- **KHÔNG CẦN:**
  - Animator component ✗
  - Animator Controller ✗
  - Animation clips ✗
- **Optional components:**
  - AnimalBehavior (Friendly) - Optional
  - AnimalInteraction (Can Be Fed, Can Be Tamed, Can Be Petted) - Optional

#### 5.2.4 Cat prefab (Pet - Curious - KHÔNG NẰM):
- **Tên: `Cat`**
- **Sprite: Cat/SpriteSheet**
- **Animal Type: Cat**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 0.7, 0.7)
  - Rigidbody2D ✓ (Linear Damping: 1.5, Angular Damping: 3)
  - Animal script ✓ (Move Speed: 3, Health: 60, Has Lying Animation: ✗)
- **Optional components:**
  - AnimalBehavior (Curious) - Optional
  - AnimalInteraction (Can Be Fed, Can Be Petted) - Optional

#### 5.2.5 Lion prefab (Wild Animal - Aggressive - CÓ NẰM):
- **Tên: `Lion`**
- **Sprite: Lion/SpriteSheetOrange** (màu cam cơ bản)
- **Animal Type: Lion**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 1.5, 1.5)
  - Rigidbody2D ✓ (Linear Damping: 2, Angular Damping: 3)
  - Animal script ✓ (Move Speed: 2, Health: 200, Has Lying Animation: ✓, Lying Delay: 3)
  - Animator ✓ (với Lying animation)
- **Optional components:**
  - AnimalBehavior (Aggressive) - Optional
  - AnimalInteraction (không cần) - Optional

#### 5.2.6 Horse prefab (Farm Animal - Strong - CÓ NẰM):
- **Tên: `Horse`**
- **Sprite: Horse/SpriteSheetBrown** (màu nâu cơ bản)
- **Animal Type: Horse**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 1.3, 1.3)
  - Rigidbody2D ✓ (Linear Damping: 3, Angular Damping: 5)
  - Animal script ✓ (Move Speed: 4, Health: 180, Has Lying Animation: ✓, Lying Delay: 4)
  - Animator ✓ (với Lying animation)
- **Optional components:**
  - AnimalBehavior (Friendly) - Optional
  - AnimalInteraction (Can Be Fed, Can Be Tamed, Can Be Ridden) - Optional

#### 5.2.7 Pig prefab (Farm Animal - Greedy - KHÔNG NẰM):
- **Tên: `Pig`**
- **Sprite: Pig/SpriteSheetPink** (màu hồng cơ bản)
- **Animal Type: Pig**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 1, 1)
  - Rigidbody2D ✓ (Linear Damping: 2.5, Angular Damping: 4)
  - Animal script ✓ (Move Speed: 1.8, Health: 120, Has Lying Animation: ✗)
- **Optional components:**
  - AnimalBehavior (Passive) - Optional
  - AnimalInteraction (Can Be Fed, Can Be Tamed) - Optional

#### 5.2.8 Fish prefab (Water Animal - Fast):
- **Tên: `Fish`**
- **Sprite: Fish/SpriteSheetRed** (màu đỏ cơ bản)
- **Animal Type: Fish**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 0.6, 0.6)
  - Rigidbody2D ✓ (Linear Damping: 1, Angular Damping: 2)
  - Animal script ✓ (Move Speed: 4, Health: 30)
- **Optional components:**
  - AnimalBehavior (Curious) - Optional
  - AnimalInteraction (Can Be Fed) - Optional

#### 5.2.9 Monkey prefab (Wild Animal - Playful):
- **Tên: `Monkey`**
- **Sprite: Monkey/SpriteSheetBrown** (màu nâu cơ bản)
- **Animal Type: Monkey**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 0.8, 0.8)
  - Rigidbody2D ✓ (Linear Damping: 1.5, Angular Damping: 2.5)
  - Animal script ✓ (Move Speed: 3.5, Health: 70)
- **Optional components:**
  - AnimalBehavior (Curious) - Optional
  - AnimalInteraction (Can Be Fed, Can Be Petted) - Optional

#### 5.2.10 Frog prefab (Wild Animal - Jumpy):
- **Tên: `Frog`**
- **Sprite: Frog/SpriteSheet**
- **Animal Type: Frog**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 0.5, 0.5)
  - Rigidbody2D ✓ (Linear Damping: 2, Angular Damping: 3)
  - Animal script ✓ (Move Speed: 2.5, Health: 40)
- **Optional components:**
  - AnimalBehavior (Scared) - Optional
  - AnimalInteraction (Can Be Fed) - Optional

#### 5.2.11 Parrot prefab (Pet - Colorful):
- **Tên: `Parrot`**
- **Sprite: Parrot/SpriteSheetBlue** (màu xanh cơ bản)
- **Animal Type: Parrot**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 0.6, 0.6)
  - Rigidbody2D ✓ (Linear Damping: 1, Angular Damping: 2)
  - Animal script ✓ (Move Speed: 3, Health: 50)
- **Optional components:**
  - AnimalBehavior (Friendly) - Optional
  - AnimalInteraction (Can Be Fed, Can Be Tamed) - Optional

#### 5.2.12 Donkey prefab (Farm Animal - Stubborn - CÓ NẰM):
- **Tên: `Donkey`**
- **Sprite: Donkey/SpriteSheetGrey**
- **Animal Type: Donkey**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 1.1, 1.1)
  - Rigidbody2D ✓ (Linear Damping: 4, Angular Damping: 6)
  - Animal script ✓ (Move Speed: 1.5, Health: 160, Has Lying Animation: ✓, Lying Delay: 5)
  - Animator ✓ (với Lying animation)
- **Optional components:**
  - AnimalBehavior (Passive) - Optional
  - AnimalInteraction (Can Be Fed, Can Be Tamed) - Optional

#### 5.2.13 Racoon prefab (Wild Animal - Mischievous):
- **Tên: `Racoon`**
- **Sprite: Racoon/SpriteSheet**
- **Animal Type: Racoon**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 0.7, 0.7)
  - Rigidbody2D ✓ (Linear Damping: 2, Angular Damping: 3)
  - Animal script ✓ (Move Speed: 2.8, Health: 65)
- **Optional components:**
  - AnimalBehavior (Curious) - Optional
  - AnimalInteraction (Can Be Fed) - Optional

#### 5.2.14 Cub prefab (Wild Animal - Young):
- **Tên: `Cub`**
- **Sprite: Cub/SpriteSheet**
- **Animal Type: Cub**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 0.8, 0.8)
  - Rigidbody2D ✓ (Linear Damping: 1.5, Angular Damping: 2.5)
  - Animal script ✓ (Move Speed: 2.2, Health: 80)
- **Optional components:**
  - AnimalBehavior (Curious) - Optional
  - AnimalInteraction (Can Be Fed, Can Be Petted) - Optional

#### 5.2.15 CatCyclop prefab (Special - Unique):
- **Tên: `CatCyclop`**
- **Sprite: CatCyclop/SpriteSheet**
- **Animal Type: CatCyclop**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 0.7, 0.7)
  - Rigidbody2D ✓ (Linear Damping: 1.5, Angular Damping: 3)
  - Animal script ✓ (Move Speed: 3.2, Health: 70)
- **Optional components:**
  - AnimalBehavior (Curious) - Optional
  - AnimalInteraction (Can Be Fed, Can Be Petted) - Optional

#### 5.2.16 Dog2 prefab (Pet - Alternative):
- **Tên: `Dog2`**
- **Sprite: Dog2/SpriteSheet**
- **Animal Type: Dog2**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 1, 1)
  - Rigidbody2D ✓ (Linear Damping: 2, Angular Damping: 4)
  - Animal script ✓ (Move Speed: 2.3, Health: 85)
- **Optional components:**
  - AnimalBehavior (Friendly) - Optional
  - AnimalInteraction (Can Be Fed, Can Be Tamed, Can Be Petted) - Optional

#### 5.2.17 Lioness prefab (Wild Animal - Female - CÓ NẰM):
- **Tên: `Lioness`**
- **Sprite: Lioness/SpriteSheet** (màu cơ bản)
- **Animal Type: Lioness**
- **Components cần thiết:**
  - SpriteRenderer ✓
  - BoxCollider2D ✓ (Size: 1.4, 1.4)
  - Rigidbody2D ✓ (Linear Damping: 2, Angular Damping: 3)
  - Animal script ✓ (Move Speed: 2.2, Health: 180, Has Lying Animation: ✓, Lying Delay: 3)
  - Animator ✓ (với Lying animation)
- **Optional components:**
  - AnimalBehavior (Aggressive) - Optional
  - AnimalInteraction (không cần) - Optional

### 5.3 Tóm tắt Animation Settings:

#### 🐄 **Animals CÓ Animation Nằm (5 loài):**
| Loài | Has Lying Animation | Lying Delay | Animator | Lý do |
|------|-------------------|-------------|----------|-------|
| **Cow** | ✓ | 4 giây | ✓ | Farm animal lớn, nghỉ ngơi |
| **Donkey** | ✓ | 5 giây | ✓ | Farm animal lớn, chậm chạp |
| **Horse** | ✓ | 4 giây | ✓ | Farm animal lớn, mạnh mẽ |
| **Lion** | ✓ | 3 giây | ✓ | Wild animal lớn, săn mồi |
| **Lioness** | ✓ | 3 giây | ✓ | Wild animal lớn, săn mồi |

#### 🐕 **Animals KHÔNG CÓ Animation Nằm (12 loài):**
| Loài | Has Lying Animation | Lý do |
|------|-------------------|-------|
| **Chicken** | ✗ | Farm animal nhỏ, luôn di chuyển |
| **Pig** | ✗ | Farm animal nhỏ, luôn di chuyển |
| **Cat** | ✗ | Pet nhỏ, năng động |
| **Dog** | ✗ | Pet nhỏ, năng động |
| **Dog2** | ✗ | Pet nhỏ, năng động |
| **Parrot** | ✗ | Pet nhỏ, bay lượn |
| **CatCyclop** | ✗ | Pet đặc biệt, năng động |
| **Cub** | ✗ | Wild animal nhỏ, chơi đùa |
| **Monkey** | ✗ | Wild animal nhỏ, leo trèo |
| **Racoon** | ✗ | Wild animal nhỏ, tinh nghịch |
| **Fish** | ✗ | Water animal, luôn bơi |
| **Frog** | ✗ | Special animal, nhảy nhót |

### 5.4 Lưu ý khi tạo prefabs:

#### 🎬 **Animals CẦN Animation (5 loài):**
| Loài | Animator | Animation Clips | Lý do |
|------|----------|-----------------|-------|
| **Cow** | ✓ | Idle, Walking, Lying | Có sprite nằm |
| **Donkey** | ✓ | Idle, Walking, Lying | Có sprite nằm |
| **Horse** | ✓ | Idle, Walking, Lying | Có sprite nằm |
| **Lion** | ✓ | Idle, Walking, Lying | Có sprite nằm |
| **Lioness** | ✓ | Idle, Walking, Lying | Có sprite nằm |

#### 🐕 **Animals KHÔNG CẦN Animation (12 loài):**
| Loài | Animator | Animation Clips | Lý do |
|------|----------|-----------------|-------|
| **Chicken** | ✗ | ✗ | Không có sprite nằm |
| **Pig** | ✗ | ✗ | Không có sprite nằm |
| **Cat** | ✗ | ✗ | Không có sprite nằm |
| **Dog** | ✗ | ✗ | Không có sprite nằm |
| **Dog2** | ✗ | ✗ | Không có sprite nằm |
| **Parrot** | ✗ | ✗ | Không có sprite nằm |
| **CatCyclop** | ✗ | ✗ | Không có sprite nằm |
| **Cub** | ✗ | ✗ | Không có sprite nằm |
| **Monkey** | ✗ | ✗ | Không có sprite nằm |
| **Racoon** | ✗ | ✗ | Không có sprite nằm |
| **Fish** | ✗ | ✗ | Không có sprite nằm |
| **Frog** | ✗ | ✗ | Không có sprite nằm |

#### 📋 **Setup cho từng loại:**

##### **Cho Animals CẦN Animation:**
- **Animator component: ✓**
- **Animator Controller: ✓**
- **Animation clips: ✓** (Idle, Walking, Lying)
- **Has Lying Animation: ✓**
- **Setup Animator: ✓** (theo hướng dẫn chi tiết)

##### **Cho Animals KHÔNG CẦN Animation:**
- **Animator component: ✗**
- **Animator Controller: ✗**
- **Animation clips: ✗**
- **Has Lying Animation: ✗**
- **Chỉ cần SpriteRenderer** thay đổi sprite cơ bản

#### 🎯 **Lưu ý quan trọng:**
- **Chỉ tạo animation cho 5 loài có sprite nằm**
- **12 loài khác KHÔNG CẦN animation**
- **Tiết kiệm thời gian và công sức**
- **Tập trung vào 5 loài quan trọng**

### 5.4 Setup Animation cho từng loài animals:

#### 5.4.1 Animals CÓ Animation Nằm (Cow, Donkey, Horse, Lion, Lioness):
- **Has Lying Animation: ✓** (nằm khi nghỉ ngơi)
- **Lying Threshold: 0.1** (chậm hơn sẽ nằm)
- **Lying Delay: 3-5 giây** (chờ lâu hơn trước khi nằm)
- **Animation States:** Idle, Walking, Lying, Eating (cho farm animals), Hunting (cho wild animals)

#### 5.4.2 Animals KHÔNG CÓ Animation Nằm (Tất cả loài khác):
- **Chicken, Pig:** Farm animals nhỏ, không nằm
- **Cat, Dog, Dog2, Parrot:** Pets, luôn di chuyển
- **Cub, Monkey, Racoon:** Wild animals nhỏ, không nằm
- **Fish, Frog:** Special animals, luôn di chuyển
- **CatCyclop:** Pet đặc biệt, không nằm
- **Has Lying Animation: ✗** (tắt tính năng nằm)
- **Animator: KHÔNG CẦN** (không tạo Animator Controller)
- **Animation States:** Chỉ cần SpriteRenderer thay đổi sprite

#### 5.4.5 Tạo Animation Clips:
1. **Tạo Animation cho mỗi state:**
   - **Idle:** Sprite đứng yên
   - **Walking:** Sprite đi bộ (loop)
   - **Lying:** Sprite nằm xuống
   - **Eating:** Sprite ăn (cho farm animals)
   - **Playing:** Sprite chơi (cho pets)
2. **Setup Animation Transitions:**
   - **Smooth transitions** giữa các states
   - **Exit Time:** 0.9 (chuyển khi gần hết animation)
   - **Transition Duration:** 0.2-0.5 giây
3. **Test Animation:**
   - Play scene và quan sát animals
   - Kiểm tra transitions mượt mà
   - Điều chỉnh timing nếu cần

## Bước 5.5: Tạo Drop Item Prefabs (Cho tính năng drop items)

### 5.5.1 Tạo Food Item (Meat):
1. **Create Empty GameObject → đặt tên `Meat`**
2. **Add Component → Sprite Renderer:**
   - Gán sprite cho meat (có thể dùng sprite đơn giản)
   - Sorting Order: 2 (hiển thị trên animals)
3. **Add Component → Scripts → Animal → AnimalDropItem:**
   - Item Name: "Meat"
   - Item Type: Food
   - Value: 1
   - Lifetime: 30
4. **Add Component → Rigidbody2D:**
   - Gravity Scale: 1
   - Linear Damping: 1
   - Angular Damping: 2
5. **Add Component → Circle Collider 2D:**
   - Is Trigger: ✓
   - Radius: 0.5
6. **Tạo prefab:** `Assets/_GAME_/Animal/Visuals/Prefab/Items/Meat.prefab`

### 5.5.2 Tạo Material Item (Leather):
1. **Create Empty GameObject → đặt tên `Leather`**
2. **Add Component → Sprite Renderer:**
   - Gán sprite cho leather
   - Sorting Order: 2
3. **Add Component → Scripts → Animal → AnimalDropItem:**
   - Item Name: "Leather"
   - Item Type: Material
   - Value: 2
   - Lifetime: 30
4. **Add Component → Rigidbody2D:**
   - Gravity Scale: 1
   - Linear Damping: 1
   - Angular Damping: 2
5. **Add Component → Circle Collider 2D:**
   - Is Trigger: ✓
   - Radius: 0.5
6. **Tạo prefab:** `Assets/_GAME_/Animal/Visuals/Prefab/Items/Leather.prefab`

### 5.5.3 Tạo Rare Item (Gem):
1. **Create Empty GameObject → đặt tên `Gem`**
2. **Add Component → Sprite Renderer:**
   - Gán sprite cho gem
   - Sorting Order: 2
3. **Add Component → Scripts → Animal → AnimalDropItem:**
   - Item Name: "Gem"
   - Item Type: Special
   - Value: 10
   - Lifetime: 30
4. **Add Component → Rigidbody2D:**
   - Gravity Scale: 1
   - Linear Damping: 1
   - Angular Damping: 2
5. **Add Component → Circle Collider 2D:**
   - Is Trigger: ✓
   - Radius: 0.5
6. **Tạo prefab:** `Assets/_GAME_/Animal/Visuals/Prefab/Items/Gem.prefab`

### 5.5.4 Setup AnimalAreaManager với Drop Items:
1. **Select AnimalAreaManager GameObject**
2. **Trong AnimalAreaManager script:**
   - **Common Items:** kéo Meat và Leather prefabs
   - **Rare Items:** kéo Gem prefab
   - **Common Drop Chances:** 0.7, 0.3 (70% Meat, 30% Leather)
   - **Rare Drop Chances:** 0.3, 0.7 (30% Gem, 70% Gem)
   - **Min Drop Amount:** 1
   - **Max Drop Amount:** 3

## Bước 6: Setup AnimalPool với Prefabs

### 6.1 Mở AnimalPool script:
1. **Select AnimalPool GameObject**
2. **Trong Inspector, tìm AnimalPool script**
3. **Trong Animal Types list, click + để thêm AnimalData**

### 6.2 Thêm Cat AnimalData:
1. **Click + trong Animal Types list**
2. **Animal Type: Cat**
3. **Prefab: kéo Cat prefab từ Project window**
4. **Sprite: kéo Cat sprite từ Project window**
5. **Spawn Weight: 1** (tỷ lệ xuất hiện)

### 6.3 Thêm Chicken AnimalData:
1. **Click + trong Animal Types list**
2. **Animal Type: Chicken**
3. **Prefab: kéo Chicken prefab**
4. **Sprite: kéo Chicken sprite**
5. **Spawn Weight: 2** (chicken xuất hiện nhiều hơn)

### 6.4 Thêm Cow AnimalData:
1. **Click + trong Animal Types list**
2. **Animal Type: Cow**
3. **Prefab: kéo Cow prefab**
4. **Sprite: kéo Cow sprite**
5. **Spawn Weight: 1**

### 6.5 Thêm Dog AnimalData:
1. **Click + trong Animal Types list**
2. **Animal Type: Dog**
3. **Prefab: kéo Dog prefab**
4. **Sprite: kéo Dog sprite**
5. **Spawn Weight: 1**

### 6.6 Thêm Lion AnimalData:
1. **Click + trong Animal Types list**
2. **Animal Type: Lion**
3. **Prefab: kéo Lion prefab**
4. **Sprite: kéo Lion sprite**
5. **Spawn Weight: 0.5** (lion xuất hiện ít hơn)

### 6.7 Lặp lại cho các loại khác:
- **Horse, Monkey, Pig, Fish, Frog, Parrot, Racoon, etc.**
- **Mỗi loại thêm 1 AnimalData**
- **Điều chỉnh Spawn Weight theo ý muốn**

### 6.8 Kiểm tra setup:
1. **Animal Types list có đủ các loại**
2. **Mỗi AnimalData có đầy đủ:**
   - Animal Type ✓
   - Prefab ✓
   - Sprite ✓
   - Spawn Weight > 0 ✓

## Bước 7: Kết nối các Components

### 7.1 Kết nối AnimalManager với AnimalSpawner:
1. **Select AnimalManager GameObject**
2. **Trong AnimalManager script, tìm Spawners list**
3. **Click + trong Spawners list**
4. **Kéo AnimalSpawner GameObject từ Hierarchy vào field vừa tạo**
5. **Kiểm tra: Spawners list hiển thị AnimalSpawner**

### 7.2 Kiểm tra connections:
1. **AnimalPool → AnimalManager:**
   - Tự động kết nối
   - Không cần setup thủ công

2. **AnimalSpawner → AnimalManager:**
   - Đã setup ở bước 7.1
   - Kiểm tra Spawners list có AnimalSpawner

3. **AnimalSpawner → AnimalPool:**
   - Tự động kết nối
   - Không cần setup thủ công

### 7.3 Kiểm tra tất cả connections:
1. **AnimalPool:**
   - Pool Size = 50 ✓
   - Pool Parent được gán ✓
   - Animal Types list có data ✓

2. **AnimalManager:**
   - Show Debug Info = true ✓
   - Global Spawning Enabled = true ✓
   - Spawners list có AnimalSpawner ✓

3. **AnimalSpawner:**
   - Spawn On Start = true ✓
   - Continuous Spawning = true ✓
   - Spawn Interval = 2 ✓
   - Max Animals = 20 ✓
   - Spawn Center được gán ✓

## Bước 8: Test System

### 8.1 Test cơ bản:
1. **Play scene:**
   - Nhấn Play button
   - Animals sẽ tự động spawn sau 2 giây
   - Kiểm tra console logs (Window → General → Console)
   - Điều chỉnh settings nếu cần

2. **Debug UI:**
   - Enable Show Debug Info trong AnimalManager
   - Debug UI sẽ hiển thị trên màn hình
   - Sử dụng Debug UI để control system
   - Kiểm tra Hierarchy để xem animals được spawn

### 8.2 Test từng bước chi tiết:

#### 8.2.1 Test Console (30 giây):
1. **Mở Console: Window → General → Console**
2. **Play scene**
3. **Kiểm tra logs:**
   - Không có error messages (màu đỏ)
   - Có thể có warning messages (màu vàng)
   - Nếu có error, dừng và fix trước khi tiếp tục

#### 8.2.2 Test Animal Spawning (2 phút):
1. **Play scene**
2. **Đếm animals theo thời gian:**
   - 0 giây: 0 animals
   - 2 giây: 1 animal
   - 4 giây: 2 animals
   - 6 giây: 3 animals
   - 8 giây: 4 animals
   - Tiếp tục cho đến Max Animals (20)
3. **Kiểm tra animals xuất hiện:**
   - Có sprite hiển thị
   - Có di chuyển
   - Không bị stuck

#### 8.2.3 Test Animal Movement (1 phút):
1. **Quan sát animals di chuyển:**
   - Animals di chuyển ngẫu nhiên
   - Không bị stuck ở một chỗ
   - Tốc độ di chuyển ổn định
2. **Kiểm tra collision:**
   - Animals không đi xuyên qua nhau
   - Collision detection hoạt động

#### 8.2.4 Test Debug UI (1 phút):
1. **Enable Show Debug Info trong AnimalManager**
2. **Debug UI hiển thị:**
   - Active Animals: số lượng tăng dần
   - Spawners: 1
   - Global Spawning: true
   - Animals Can Move: true
3. **Sử dụng Debug UI buttons:**
   - Start All Spawning
   - Stop All Spawning
   - Clear All Animals

#### 8.2.5 Test Performance (2 phút):
1. **Monitor FPS:**
   - Window → Analysis → Profiler
   - FPS không được dưới 30
   - Không có lag khi có nhiều animals
2. **Memory usage:**
   - Không có memory leak
   - Object pooling hoạt động đúng
3. **Stress test:**
   - Tăng Max Animals lên 50
   - Play scene và chờ đủ 50 animals
   - Kiểm tra performance

#### 8.2.6 Test Spawn Area (1 phút):
1. **Kiểm tra spawn radius:**
   - Animals spawn trong vòng tròn
   - Không spawn quá xa hoặc quá gần
   - Spawn area hiển thị trong Scene view
2. **Test với Player:**
   - Nếu có Player trong scene
   - Animals không spawn quá gần player
   - Min Distance From Player hoạt động

#### 8.2.7 Test Area Movement (2 phút):
1. **Kiểm tra animals di chuyển trong vùng:**
   - Animals không ra khỏi vùng tròn
   - Tự động quay lại khi ra khỏi vùng
   - Thay đổi hướng ngẫu nhiên theo interval
2. **Kiểm tra Area Gizmos:**
   - Vùng tròn hiển thị trong Scene view
   - Màu vùng đúng (Green)
   - Bán kính vùng đúng

#### 8.2.8 Test Drop Items (3 phút):
1. **Giết animals để test drop:**
   - Select animal trong Hierarchy
   - Trong Inspector, giảm Health xuống 0
   - Hoặc thêm script để giết animals
2. **Quan sát items drop:**
   - Items xuất hiện tại vị trí animal
   - Items có physics (rơi, nảy)
   - Số lượng items đúng (1-3 items)
   - Items có lifetime (biến mất sau 30 giây)
3. **Test pickup items:**
   - Di chuyển player đến gần items
   - Items sẽ được pickup tự động
   - Console hiển thị "Player picked up: [ItemName] x[Amount]"

#### 8.2.9 Test Lying Animation (2 phút):
1. **Kiểm tra animals nằm khi đứng yên:**
   - Quan sát animals di chuyển bình thường
   - Khi animals dừng lại, chờ 2 giây
   - Animals sẽ chuyển sang animation nằm
2. **Kiểm tra animals đứng dậy khi di chuyển:**
   - Khi animals bắt đầu di chuyển
   - Animals sẽ chuyển từ nằm sang đứng
   - Animation chuyển đổi mượt mà
3. **Test Animator parameters:**
   - Mở Animator window
   - Quan sát IsMoving và IsLying parameters
   - Parameters thay đổi đúng theo trạng thái

### Test nhanh (5 phút):
1. **Bước 1: Kiểm tra Console**
   - Play scene
   - Mở Console (Window → General → Console)
   - Không có error messages = OK

2. **Bước 2: Kiểm tra Animals spawn**
   - Sau 2 giây: 1 animal xuất hiện
   - Sau 4 giây: 2 animals xuất hiện
   - Sau 6 giây: 3 animals xuất hiện
   - Tiếp tục cho đến Max Animals

3. **Bước 3: Kiểm tra Animals di chuyển**
   - Animals di chuyển ngẫu nhiên
   - Không bị stuck ở một chỗ
   - Tốc độ di chuyển ổn định

4. **Bước 4: Kiểm tra Performance**
   - FPS không dưới 30
   - Không có lag khi có nhiều animals
   - Memory usage ổn định

5. **Bước 5: Kiểm tra Debug UI**
   - Enable Show Debug Info trong AnimalManager
   - Debug UI hiển thị trên màn hình
   - Số lượng Active Animals tăng dần

### Test thủ công chi tiết:

#### 1. Kiểm tra AnimalPool:
- **Mở Console (Window → General → Console)**
- **Play scene và xem logs:**
  - Nếu thấy "AnimalPool initialized" → OK
  - Nếu thấy "No prefabs found" → Cần setup prefabs
  - Nếu thấy "Pool size: X" → Pool hoạt động

#### 2. Kiểm tra AnimalSpawner:
- **Trong Inspector của AnimalSpawner:**
  - Spawn On Start: ✓ (checked)
  - Continuous Spawning: ✓ (checked)
  - Spawn Interval: 2 (giây)
  - Max Animals: 20

- **Play scene và đếm animals:**
  - Sau 2 giây đầu tiên: 1 animal
  - Sau 4 giây: 2 animals
  - Sau 6 giây: 3 animals
  - Tiếp tục cho đến Max Animals

#### 3. Kiểm tra AnimalManager:
- **Enable Show Debug Info:**
  - Checkbox "Show Debug Info" = ✓
  - Sẽ hiển thị Debug UI trên màn hình

- **Debug UI sẽ hiển thị:**
  - Active Animals: số lượng animals hiện tại
  - Spawners: số lượng spawners
  - Global Spawning: trạng thái spawning
  - Animals Can Move: trạng thái di chuyển

#### 4. Test Object Pooling:
- **Tạo nhiều animals:**
  - Tăng Max Animals lên 50
  - Play scene và chờ đủ 50 animals
  - Kiểm tra performance (không lag)

- **Test return to pool:**
  - Tạm dừng spawning (uncheck Continuous Spawning)
  - Chờ animals tự động return về pool
  - Kiểm tra số lượng animals giảm

#### 5. Test Animal Behavior:
- **Kiểm tra di chuyển:**
  - Animals phải di chuyển ngẫu nhiên
  - Không bị stuck ở một chỗ
  - Tốc độ di chuyển ổn định

- **Kiểm tra collision:**
  - Animals không đi xuyên qua nhau
  - Collision detection hoạt động

#### 6. Test Performance:
- **Monitor FPS:**
  - Window → Analysis → Profiler
  - Kiểm tra FPS khi có nhiều animals
  - FPS không được dưới 30

- **Memory usage:**
  - Không có memory leak
  - Object pooling hoạt động đúng

#### 7. Test Spawn Area:
- **Kiểm tra spawn radius:**
  - Animals spawn trong vòng tròn
  - Không spawn quá xa hoặc quá gần
  - Spawn area hiển thị trong Scene view

#### 8. Test với Player:
- **Nếu có Player trong scene:**
  - Animals không spawn quá gần player
  - Min Distance From Player hoạt động
  - Không có conflict với player

### Troubleshooting Test:

#### Nếu animals không spawn:
1. **Kiểm tra AnimalPool:**
   - Có prefabs trong Animal Types list không?
   - Prefabs có được gán đúng không?
   - Pool Size > 0?

2. **Kiểm tra AnimalSpawner:**
   - Spawn On Start = true?
   - Continuous Spawning = true?
   - Spawn Interval > 0?
   - Max Animals > 0?

3. **Kiểm tra Console:**
   - Có error messages không?
   - Có warning messages không?

#### Nếu animals không di chuyển:
1. **Kiểm tra Animal script:**
   - Is Moving = true?
   - Move Speed > 0?
   - Rigidbody2D có đúng settings không?

2. **Kiểm tra Physics:**
   - Gravity Scale = 0?
   - Linear Damping = 2?
   - Collision detection hoạt động?

#### Nếu performance kém:
1. **Giảm Max Animals:**
   - Từ 50 xuống 20
   - Từ 20 xuống 10

2. **Tăng Spawn Interval:**
   - Từ 2 giây lên 5 giây
   - Từ 5 giây lên 10 giây

3. **Kiểm tra Object Pooling:**
   - Pool Size phù hợp
   - Animals được return về pool đúng cách

## Bước 9: Tùy chỉnh (Optional)

1. **Thay đổi spawn settings:**
   - Spawn Interval: thời gian giữa các lần spawn
   - Max Animals: số lượng tối đa
   - Spawn Radius: bán kính spawn area

2. **Thay đổi animal behavior:**
   - Move Speed: tốc độ di chuyển
   - Health: máu
   - Movement patterns

3. **Thêm logic tương tác:**
   - Player interaction
   - AI behavior
   - Animation

## Troubleshooting Chi tiết

### 9.1 Animals không spawn:

#### 9.1.1 Kiểm tra AnimalPool:
1. **AnimalPool có prefabs không?**
   - Mở AnimalPool script
   - Kiểm tra Animal Types list có data không
   - Mỗi AnimalData có đầy đủ: Animal Type, Prefab, Sprite, Spawn Weight > 0

2. **Prefabs có được gán đúng không?**
   - Kiểm tra Prefab field không null
   - Kiểm tra Prefab có Animal script không
   - Kiểm tra Prefab có SpriteRenderer không

3. **Pool Size > 0?**
   - Pool Size phải > 0
   - Pool Parent được gán

#### 9.1.2 Kiểm tra AnimalSpawner:
1. **Spawn On Start = true?**
2. **Continuous Spawning = true?**
3. **Spawn Interval > 0?**
4. **Max Animals > 0?**
5. **Spawn Center được gán?**

#### 9.1.3 Kiểm tra Console:
1. **Có error messages không?**
   - Màu đỏ = Error (phải fix)
   - Màu vàng = Warning (có thể bỏ qua)
2. **Có warning messages không?**
3. **Logs hiển thị gì?**

### 9.2 Animals không di chuyển:

#### 9.2.1 Kiểm tra Animal script:
1. **Is Moving = true?**
2. **Move Speed > 0?**
3. **Rigidbody2D có đúng settings không?**
   - Gravity Scale = 0
   - Drag = 2
   - Angular Drag = 5

#### 9.2.2 Kiểm tra AnimalBehavior script:
1. **AnimalBehavior script có được thêm không?**
2. **Can Move = true?**
3. **Move Speed > 0?**
4. **Behavior Type được set đúng?**

#### 9.2.3 Kiểm tra Physics:
1. **Gravity Scale = 0?**
2. **Drag = 2?**
3. **Collision detection hoạt động?**
4. **Animals không bị stuck?**

### 9.3 Sprites không hiển thị:

#### 9.3.1 Kiểm tra sprite import settings:
1. **Sprite Mode = Multiple?**
2. **Pixels Per Unit = 32?**
3. **Filter Mode = Point?**
4. **Compression = None?**

#### 9.3.2 Kiểm tra SpriteRenderer component:
1. **SpriteRenderer có được thêm không?**
2. **Sprite field có được gán không?**
3. **Sorting Order = 1?**
4. **Color = White?**

#### 9.3.3 Kiểm tra sorting order:
1. **Sorting Order = 1?**
2. **Sorting Layer = Default?**
3. **Camera có đúng settings không?**

### 9.4 Performance issues:

#### 9.4.1 Giảm Max Animals:
1. **Từ 50 xuống 20**
2. **Từ 20 xuống 10**
3. **Test performance sau mỗi lần giảm**

#### 9.4.2 Tăng Spawn Interval:
1. **Từ 2 giây lên 5 giây**
2. **Từ 5 giây lên 10 giây**
3. **Test performance sau mỗi lần tăng**

#### 9.4.3 Kiểm tra Object Pooling:
1. **Pool Size phù hợp**
2. **Animals được return về pool đúng cách**
3. **Không có memory leak**

### 9.5 Debug System:

#### 9.5.1 Enable Debug UI:
1. **AnimalManager → Show Debug Info = true**
2. **Debug UI hiển thị trên màn hình**
3. **Sử dụng Debug UI để monitor system**

#### 9.5.2 Console Debug:
1. **Mở Console: Window → General → Console**
2. **Kiểm tra logs**
3. **Fix errors trước khi tiếp tục**

#### 9.5.3 Hierarchy Debug:
1. **Kiểm tra animals được spawn trong Hierarchy**
2. **Kiểm tra components của animals**
3. **Kiểm tra connections giữa components**

### 9.6 Common Issues:

#### 9.6.1 "No prefabs found":
- **Nguyên nhân:** AnimalPool không có prefabs
- **Giải pháp:** Thêm prefabs vào Animal Types list

#### 9.6.2 "AnimalPool not found":
- **Nguyên nhân:** AnimalPool GameObject không có AnimalPool script
- **Giải pháp:** Thêm AnimalPool script vào GameObject

#### 9.6.3 "AnimalSpawner not found":
- **Nguyên nhân:** AnimalSpawner GameObject không có AnimalSpawner script
- **Giải pháp:** Thêm AnimalSpawner script vào GameObject

#### 9.6.4 Animals spawn nhưng không di chuyển:
- **Nguyên nhân:** Thiếu AnimalBehavior script hoặc settings sai
- **Giải pháp:** Thêm AnimalBehavior script và setup đúng

#### 9.6.5 Performance lag:
- **Nguyên nhân:** Quá nhiều animals hoặc settings không tối ưu
- **Giải pháp:** Giảm Max Animals, tăng Spawn Interval, kiểm tra Object Pooling

## Cấu trúc cuối cùng:

```
Hierarchy:
├── AnimalPool
│   └── PoolParent
├── AnimalManager
├── AnimalSpawner
└── AnimalAreaManager (Optional - cho vùng di chuyển và drop items)

Assets/_GAME_/Animal/:
├── Script/
│   ├── Animal.cs                    # Script cơ bản cho từng con vật
│   ├── AnimalPool.cs                 # Object pooling system
│   ├── AnimalSpawner.cs              # Hệ thống spawn animals
│   ├── AnimalManager.cs              # Quản lý toàn bộ hệ thống
│   ├── AnimalAreaManager.cs          # Quản lý vùng di chuyển và drop items - Optional
│   ├── AnimalDropItem.cs             # Script cho drop items - Optional
│   ├── AnimalVariants.cs             # Quản lý biến thể (màu sắc) - Optional
│   ├── AnimalSpawnConfig.cs          # Cấu hình spawn nâng cao - Optional
│   ├── AnimalBehavior.cs             # AI behavior cho animals - Optional
│   └── AnimalInteraction.cs          # Tương tác với player - Optional
├── Visuals/
│   ├── Prefab/
│   │   ├── Animals/
│   │   │   ├── Cat.prefab
│   │   │   ├── Chicken.prefab
│   │   │   ├── Cow.prefab
│   │   │   ├── Dog.prefab
│   │   │   ├── Lion.prefab
│   │   │   └── ... (tất cả loại animals)
│   │   └── Items/
│   │       ├── Meat.prefab
│   │       ├── Leather.prefab
│   │       ├── Gem.prefab
│   │       └── ... (tất cả drop items)
│   └── Spirite/
│       ├── Animals/
│       │   ├── Cat/
│       │   ├── Chicken/
│       │   ├── Cow/
│       │   ├── Dog/
│       │   ├── Lion/
│       │   └── ... (tất cả loại animals)
│       └── Farm Animals/
│           ├── Baby Chicken Yellow.png
│           ├── Chicken Blonde Green.png
│           ├── Chicken Red.png
│           ├── Female Cow Brown.png
│           └── Male Cow Brown.png
├── README.md
└── SETUP_MANUAL.md
```

## Tóm tắt Setup:

### ✅ **Bước 1-3: Tạo GameObjects và Scripts**
- AnimalPool + AnimalPool script
- AnimalManager + AnimalManager script  
- AnimalSpawner + AnimalSpawner script

### ✅ **Bước 4: Import Sprites**
- Copy sprites từ external folder
- Import vào Unity với đúng settings
- Slice sprites nếu cần

### ✅ **Bước 5: Tạo Prefabs**
- Tạo prefab cho mỗi loại animal
- Thêm đầy đủ components: SpriteRenderer, Collider2D, Rigidbody2D, Animal, AnimalBehavior, AnimalInteraction
- Setup đúng settings cho từng loại

### ✅ **Bước 6: Setup AnimalPool**
- Thêm AnimalData cho từng loại animal
- Gán prefabs và sprites
- Setup spawn weights

### ✅ **Bước 7: Kết nối Components**
- Kết nối AnimalManager với AnimalSpawner
- Kiểm tra tất cả connections

### ✅ **Bước 8: Test System**
- Test console, spawning, movement, performance
- Sử dụng Debug UI để monitor
- Fix các issues nếu có

### ✅ **Bước 9: Troubleshooting**
- Debug các vấn đề thường gặp
- Tối ưu performance
- Test thoroughly

## Bước 10: Thêm Features Nâng cao (Optional)

### 10.1 Thêm AnimalBehavior (AI Behavior):
1. **Thêm AnimalBehavior script vào prefabs:**
   - Chọn prefab trong Project window
   - Add Component → Scripts → Animal → AnimalBehavior
   - Setup Behavior Type cho từng loại animal
2. **Test AI behavior:**
   - Play scene
   - Quan sát animals có behavior khác nhau
   - Cat: Curious, Dog: Friendly, Lion: Aggressive

### 10.2 Thêm AnimalInteraction (Player Interaction):
1. **Thêm AnimalInteraction script vào prefabs:**
   - Chọn prefab trong Project window
   - Add Component → Scripts → Animal → AnimalInteraction
   - Setup interaction settings cho từng loại
2. **Test player interaction:**
   - Play scene
   - Tiến lại gần animals
   - Test feeding, petting, taming

### 10.3 Thêm AnimalVariants (Biến thể màu sắc):
1. **Tạo GameObject mới:**
   - Right-click trong Hierarchy → Create Empty
   - Đặt tên: `AnimalVariants`
2. **Thêm AnimalVariants script:**
   - Add Component → Scripts → Animal → AnimalVariants
   - Setup variants cho từng loại animal
3. **Test variants:**
   - Play scene
   - Quan sát animals có màu sắc khác nhau

### 10.4 Thêm AnimalSpawnConfig (Cấu hình spawn nâng cao):
1. **Tạo GameObject mới:**
   - Right-click trong Hierarchy → Create Empty
   - Đặt tên: `AnimalSpawnConfig`
2. **Thêm AnimalSpawnConfig script:**
   - Add Component → Scripts → Animal → AnimalSpawnConfig
   - Setup spawn configs cho từng loại animal
3. **Test spawn configs:**
   - Play scene
   - Quan sát spawn behavior khác nhau

### 10.5 Thêm AnimalAreaManager (Vùng di chuyển và Drop Items):
1. **Tạo GameObject mới:**
   - Right-click trong Hierarchy → Create Empty
   - Đặt tên: `AnimalAreaManager`
2. **Thêm AnimalAreaManager script:**
   - Add Component → Scripts → Animal → AnimalAreaManager
   - Setup Area Center (có thể dùng AnimalSpawner)
   - Setup Area Radius (ví dụ: 15)
3. **Setup Drop Items:**
   - Tạo prefabs cho drop items (Food, Materials, etc.)
   - Gán vào Common Items và Rare Items
   - Setup drop chances
4. **Test area movement:**
   - Play scene
   - Quan sát animals di chuyển trong vùng
   - Test animals drop items khi chết

### 10.6 Thêm AnimalDropItem (Drop Items):

#### 10.6.1 Tạo Drop Item Prefabs:
1. **Tạo Food Item (ví dụ: Meat):**
   - Create Empty GameObject → đặt tên `Meat`
   - Add Component → Sprite Renderer (gán sprite)
   - Add Component → Scripts → Animal → AnimalDropItem
   - Setup: Item Name = "Meat", Type = Food, Value = 1
   - Add Component → Rigidbody2D
   - Add Component → Circle Collider 2D (Is Trigger = true)
   - Tạo prefab: `Assets/_GAME_/Animal/Visuals/Prefab/Items/Meat.prefab`

2. **Tạo Material Item (ví dụ: Leather):**
   - Create Empty GameObject → đặt tên `Leather`
   - Add Component → Sprite Renderer (gán sprite)
   - Add Component → Scripts → Animal → AnimalDropItem
   - Setup: Item Name = "Leather", Type = Material, Value = 2
   - Add Component → Rigidbody2D
   - Add Component → Circle Collider 2D (Is Trigger = true)
   - Tạo prefab: `Assets/_GAME_/Animal/Visuals/Prefab/Items/Leather.prefab`

3. **Tạo Rare Item (ví dụ: Gem):**
   - Create Empty GameObject → đặt tên `Gem`
   - Add Component → Sprite Renderer (gán sprite)
   - Add Component → Scripts → Animal → AnimalDropItem
   - Setup: Item Name = "Gem", Type = Special, Value = 10
   - Add Component → Rigidbody2D
   - Add Component → Circle Collider 2D (Is Trigger = true)
   - Tạo prefab: `Assets/_GAME_/Animal/Visuals/Prefab/Items/Gem.prefab`

#### 10.6.2 Setup Item Physics:
1. **Rigidbody2D settings:**
   - Gravity Scale: 1 (để rơi xuống)
   - Linear Damping: 1 (ma sát di chuyển)
   - Angular Damping: 2 (ma sát xoay)
2. **Collider settings:**
   - Is Trigger: true (để pickup)
   - Size phù hợp với sprite
3. **AnimalDropItem settings:**
   - Bounce Force: 2 (độ nảy)
   - Friction: 0.95 (ma sát)
   - Use Gravity: true
   - Lifetime: 30 (giây)

#### 10.6.3 Test Drop Items:
1. **Play scene**
2. **Giết animals:**
   - Select animal trong Hierarchy
   - Trong Inspector, giảm Health xuống 0
   - Hoặc thêm script để giết animals
3. **Quan sát items drop:**
   - Items xuất hiện tại vị trí animal
   - Items có physics (rơi, nảy)
   - Items có lifetime (biến mất sau 30 giây)
4. **Test pickup:**
   - Di chuyển player đến gần items
   - Items sẽ được pickup tự động

### 10.7 Lưu ý khi thêm features nâng cao:
- **Thêm từng feature một** để dễ debug
- **Test từng feature** trước khi thêm feature khác
- **Backup project** trước khi thêm features
- **Có thể bỏ qua** nếu không cần features nâng cao

## Checklist Test:

### ✅ Setup cơ bản:
- [ ] AnimalPool GameObject được tạo
- [ ] AnimalManager GameObject được tạo  
- [ ] AnimalSpawner GameObject được tạo
- [ ] Tất cả scripts được thêm vào GameObjects
- [ ] Connections giữa components được setup

### ✅ Sprites và Prefabs:
- [ ] Sprites được import từ external folder
- [ ] Prefabs được tạo cho từng loại animal
- [ ] Prefabs được gán vào AnimalPool
- [ ] Animal Types được setup đúng

### ✅ Test chức năng:
- [ ] Animals spawn tự động khi play scene
- [ ] Animals di chuyển ngẫu nhiên
- [ ] Object pooling hoạt động (không lag)
- [ ] Debug UI hiển thị thông tin đúng
- [ ] Console không có error messages

### ✅ Performance:
- [ ] FPS ổn định (không dưới 30)
- [ ] Memory usage ổn định
- [ ] Không có memory leak
- [ ] Spawn rate phù hợp với game

## Lưu ý quan trọng:

1. **Luôn test từng bước** để đảm bảo hoạt động đúng
2. **Backup project** trước khi thay đổi lớn
3. **Sử dụng Debug UI** để monitor system
4. **Tối ưu performance** bằng cách điều chỉnh pool size và spawn rate
5. **Test thường xuyên** để đảm bảo system hoạt động ổn định
