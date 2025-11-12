# Hướng Dẫn Setup Animal

## Tổng Quan
Animal được setup tương tự như Enemy nhưng có một số khác biệt:
- **Hành vi**: Animal chạy tránh player thay vì tấn công
- **Spawn thời gian**: Animal spawn ban ngày (6h-18h), Enemy spawn ban đêm (18h-6h)
- **Drop**: Animal có thể drop loot items khi chết

---

## Bước 1: Tạo Animal Prefab

### 1.1. Tạo GameObject Animal
1. Tạo GameObject mới: `Animal` (hoặc tên cụ thể như `Chicken`, `Cow`, etc.)
2. Thêm các components sau:

#### **Components Cần Có:**
- **Transform** (mặc định)
- **SpriteRenderer**: Hiển thị sprite animal
  - Drag sprite vào `Sprite` field
  - Set `Sorting Layer` phù hợp (ví dụ: Layer 5)
- **Animator**: Nếu có animation
  - Assign Animator Controller
- **Rigidbody2D**: 
  - `Body Type`: Dynamic
  - `Gravity Scale`: 0 (không rơi)
  - `Freeze Rotation Z`: ✅ (không xoay)
  - `Linear Damping`: 0
  - `Angular Damping`: 0.05
- **CircleCollider2D**: 
  - `Radius`: 0.13 (tùy chỉnh theo size animal)
  - `Is Trigger`: ❌
- **Animal Script**: 
  - Add component: `Animal` script

### 1.2. Setup Animal Script Parameters
Trong Inspector, config các thông số:

**Stats:**
- `Speed`: 2 (tốc độ di chuyển)
- `Max Health`: 100
- `Current Health`: 100 (sẽ được reset khi spawn)
- `Exp Amount`: 50 (ít hơn enemy)
- `Flee Speed Multiplier`: 1.5 (chạy nhanh hơn khi sợ)

**Ranges:**
- `Player Detect Range`: 5 (phát hiện player từ khoảng cách này)
- `Flee Range`: 7 (khoảng cách an toàn để dừng chạy)
- `Patrol Radius`: 4 (bán kính tuần tra)
- `Max Wander Distance`: 8 (khoảng cách tối đa khỏi spawn point)

**Loot Settings:**
- `Loot Prefabs`: Array các prefab items sẽ rơi khi chết (meat, fur, etc.)
- `Loot Amounts`: Số lượng mỗi loại item (phải match với Loot Prefabs)
- `Loot Drop Chance`: 1 (100% drop)

**Knockback Settings:**
- `Knockback Force`: 5
- `Knockback Duration`: 0.2

**Player:**
- Để trống (sẽ tự tìm Player tag)

### 1.3. Tạo Health Bar UI
1. Tạo child GameObject: `HealthBar UI`
   - Add component: `RectTransform`
   - Add component: `CanvasRenderer`
   - Add component: `AnimalHealthUI` script
   - Position: `(0, 0.17, 0)` (phía trên animal)
   - Scale: `(0.2, 0.2, 1)`
   - Size: `(30, 30)`

2. Tạo child của `HealthBar UI`: `Full health`
   - Add component: `RectTransform`
   - Add component: `SpriteRenderer`
   - Sprite: Red health bar sprite
   - Position: `(0, 0, 0.021)`
   - Size: `(1.125, 0.25)`
   - Sorting Layer: 7 (cao hơn animal)

3. Tạo child của `HealthBar UI`: `Empty health`
   - Add component: `RectTransform`
   - Add component: `SpriteRenderer`
   - Sprite: Empty/background health bar sprite
   - Position: `(0, 0, -0.054)` (phía sau Full health)
   - Size: `(1.125, 0.25)`
   - Sorting Layer: 6

4. Setup AnimalHealthUI Script:
   - `Animal`: Drag Animal component vào đây
   - `Full Health Bar`: Drag `Full health` Transform
   - `Empty Health Bar`: Drag `Empty health` Transform

### 1.4. Tạo Prefab
1. Drag GameObject `Animal` vào folder `Assets/_GAME_/Animal/Visuals/Prefab/Animal/`
2. Xóa instance trong scene (nếu có)
3. Prefab đã sẵn sàng!

---

## Bước 2: Setup Animal Spawner trong Scene

### 2.1. Tạo Spawn Area
1. Tạo GameObject mới: `Animal Spawn Area`
2. Add component: `PolygonCollider2D`
3. Vẽ polygon shape cho vùng spawn (click Edit Collider trong Inspector)
4. Set `Is Trigger`: ❌ (không cần trigger)

### 2.2. Tạo Animal Spawner GameObject
1. Tạo GameObject mới: `Animal Spawn`
2. Add component: `AnimalSpawned` script

### 2.3. Setup AnimalSpawned Script
**Pooling Settings:**
- `Prefabs`: List các Animal prefabs muốn spawn
  - Size: Số lượng loại animal (ví dụ: 2 cho Chicken, Cow)
  - Element 0: Drag Chicken prefab
  - Element 1: Drag Cow prefab
- `Spawn Rates`: Tỷ lệ spawn mỗi loại (ví dụ: 0.5, 0.5 = 50% mỗi loại)
  - Size: Phải match với Prefabs size
  - Element 0: 0.5 (50% Chicken)
  - Element 1: 0.5 (50% Cow)
- `Period`: 2 (spawn mỗi 2 giây - chậm hơn enemy)
- `Pool Size`: 10 (số lượng animal trong pool)

**References:**
- `World Time`: Drag WorldTime GameObject từ scene
- `Spawn Area`: Drag `Animal Spawn Area` PolygonCollider2D

### 2.4. (Optional) Setup WorldTimeWatcher để Auto Activate/Deactivate
1. Tìm GameObject có `WorldTimeWatcher` component
2. Thêm timed events:
   - **Event 1**: "Spawn Animal"
     - Hour: 6
     - Minute: 0
     - On Triggered: `AnimalSpawned.ActivateSpawner()`
   - **Event 2**: "Despawn Animal"
     - Hour: 18
     - Minute: 0
     - On Triggered: `AnimalSpawned.DeactivateSpawner()`

**Lưu ý:** Animal sẽ tự spawn từ 6h-18h trong code, nhưng có thể dùng WorldTimeWatcher để control tốt hơn.

---

## Bước 3: Cập Nhật Weapon Script (Quan Trọng!)

Player cần có thể tấn công Animal. Cập nhật `Weapon.cs`:

```csharp
public void OnTriggerEnter2D(Collider2D collision)
{
    Enemy enemy = collision.GetComponent<Enemy>();
    Animal animal = collision.GetComponent<Animal>();

    if (enemy != null)
    {
        Transform playerTransform = transform.root;
        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerTransform != null)
        {
            enemy.TakeDamage(_damage, playerTransform.position);
        }
    }

    // Thêm xử lý cho Animal
    if (animal != null)
    {
        Transform playerTransform = transform.root;
        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerTransform != null)
        {
            animal.TakeDamage(_damage, playerTransform.position);
        }
    }
}
```

---

## Bước 4: Tạo Loot Items (Optional)

Nếu muốn animal drop items khi chết:

1. Tạo Item prefabs (nếu chưa có):
   - Meat prefab
   - Fur prefab
   - etc.

2. Trong Animal prefab Inspector:
   - `Loot Prefabs`: Drag các item prefabs
   - `Loot Amounts`: Set số lượng mỗi item (ví dụ: 1 meat, 2 fur)
   - `Loot Drop Chance`: 1 (100%)

3. Đảm bảo item prefabs có:
   - `Item` script
   - `BounceEffect` script (optional, cho hiệu ứng)
   - `Collider2D` với `Is Trigger`: ✅
   - Tag: `Item`

---

## Bước 5: Testing

1. **Test Spawn:**
   - Set WorldTime về 6h-18h (ban ngày)
   - Animal sẽ spawn trong spawn area
   - Kiểm tra pooling hoạt động (animal reuse khi chết)

2. **Test Behavior:**
   - Animal patrol khi không thấy player
   - Animal flee khi player đến gần
   - Animal return về spawn point khi player xa

3. **Test Combat:**
   - Player tấn công animal
   - Animal nhận damage và flee
   - Animal chết và drop loot (nếu có)
   - Animal respawn từ pool

4. **Test Health Bar:**
   - Health bar hiển thị đúng
   - Health bar giảm khi animal bị damage
   - Health bar ẩn khi animal chết

---

## Tóm Tắt Components Cần Có

### Animal Prefab:
- Transform
- SpriteRenderer
- Animator (optional)
- Rigidbody2D
- CircleCollider2D
- Animal script
- HealthBar UI (child)
  - RectTransform
  - CanvasRenderer
  - AnimalHealthUI script
  - Full health (child)
  - Empty health (child)

### Animal Spawner:
- Transform
- AnimalSpawned script
- Reference: WorldTime
- Reference: PolygonCollider2D (spawn area)

---

## Lưu Ý Quan Trọng

1. **Tag & Layer:**
   - Animal không cần tag đặc biệt (khác với Player tag)
   - Đảm bảo Collider2D không overlap với player layer

2. **Pooling:**
   - Animal sẽ được pool và reuse
   - Khi animal chết, gọi `SetActive(false)` để trả về pool
   - AnimalSpawned sẽ tự reset health khi spawn

3. **Performance:**
   - Không spawn quá nhiều animal cùng lúc
   - Điều chỉnh `period` và `poolSize` phù hợp
   - Sử dụng Object Pooling để tối ưu

4. **Spawn Area:**
   - Đảm bảo spawn area không overlap với player spawn
   - Spawn area nên ở vùng an toàn (không có obstacle)

---

## Troubleshooting

### Animal không spawn:
- Kiểm tra WorldTime đang ở 6h-18h (ban ngày)
- Kiểm tra spawn area có đúng không
- Kiểm tra prefabs có được assign chưa

### Animal không flee:
- Kiểm tra Player tag có đúng không
- Kiểm tra `Player Detect Range` có quá nhỏ không
- Kiểm tra player Transform có được tìm thấy không

### Animal không drop loot:
- Kiểm tra `Loot Prefabs` có được assign chưa
- Kiểm tra `Loot Drop Chance` có > 0 không
- Kiểm tra item prefabs có `Item` script chưa

### Health bar không hiển thị:
- Kiểm tra `AnimalHealthUI` script có được assign đúng không
- Kiểm tra `Full Health Bar` và `Empty Health Bar` có được assign chưa
- Kiểm tra Sorting Layer có đúng không

---

## Kết Luận

Sau khi setup xong, Animal sẽ:
- ✅ Spawn ban ngày (6h-18h)
- ✅ Patrol trong spawn area
- ✅ Flee khi player đến gần
- ✅ Drop loot khi chết
- ✅ Respawn từ pool
- ✅ Hiển thị health bar

Chúc bạn setup thành công! 🎉

