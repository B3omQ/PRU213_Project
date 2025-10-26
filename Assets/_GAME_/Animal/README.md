# Animal System - Hệ thống Spawn Animal với Object Pooling

## Tổng quan
Hệ thống Animal được thiết kế để spawn và quản lý các con vật trong game với object pooling pattern để tối ưu performance.

## Cấu trúc thư mục
```
Assets/_GAME_/Animal/
├── Script/                    # Các script chính
│   ├── Animal.cs             # Script chính cho từng con vật
│   ├── AnimalPool.cs          # Object pooling system
│   ├── AnimalSpawner.cs       # Hệ thống spawn animals
│   └── AnimalManager.cs       # Quản lý toàn bộ hệ thống
├── Visuals/
│   ├── Prefab/               # Các prefab animals (tạo thủ công)
│   └── Spirite/              # Sprites đã import (tạo thủ công)
├── README.md                 # File hướng dẫn này
└── SETUP_MANUAL.md          # Hướng dẫn setup thủ công
```

## Cách sử dụng (Setup thủ công)

### 1. Tạo GameObjects
1. Tạo 3 GameObject: `AnimalPool`, `AnimalManager`, `AnimalSpawner`
2. Thêm script tương ứng vào từng GameObject
3. Setup connections giữa các components

### 2. Import Sprites
1. Copy sprites từ `C:\Users\Admin\Downloads\PRU\Animal\Animals` vào `Assets/_GAME_/Animal/Visuals/Spirite`
2. Import sprites vào Unity
3. Slice sprites nếu cần thiết

### 3. Tạo Prefabs
1. Tạo GameObject cho mỗi loại animal
2. Thêm SpriteRenderer, Collider2D, Rigidbody2D, Animal script
3. Setup properties cho từng animal
4. Tạo prefab và lưu vào `Assets/_GAME_/Animal/Visuals/Prefab`

### 4. Setup AnimalPool
1. Thêm prefabs vào AnimalPool script
2. Setup AnimalData cho mỗi loại animal
3. Test object pooling

### 5. Spawn Animals
1. Play scene để test
2. Điều chỉnh settings trong AnimalSpawner:
   - `spawnInterval`: Thời gian giữa các lần spawn
   - `maxAnimals`: Số lượng animals tối đa
   - `spawnRadius`: Bán kính spawn area

## Các thành phần chính

### Animal.cs
- Script chính cho từng con vật
- Quản lý movement, health, behavior
- Hỗ trợ object pooling

### AnimalPool.cs
- Quản lý object pooling
- Tái sử dụng animals để tối ưu performance
- Hỗ trợ nhiều loại animals

### AnimalSpawner.cs
- Spawn animals trong scene
- Quản lý spawn area và timing
- Tự động spawn hoặc spawn theo yêu cầu

### AnimalManager.cs
- Quản lý toàn bộ hệ thống
- Debug UI
- Global settings cho animals

## Các loại Animals hỗ trợ
- Cat, CatCyclop
- Chicken (nhiều màu)
- Cow
- Cub
- Dog, Dog2
- Donkey
- Fish (nhiều màu)
- Frog
- Horse (nhiều màu)
- Lion, Lioness (nhiều màu)
- Monkey (nhiều màu)
- Parrot (nhiều màu)
- Pig (nhiều màu)
- Racoon

## Tùy chỉnh

### Thay đổi behavior
Chỉnh sửa trong `Animal.cs`:
```csharp
public float moveSpeed = 2f;        // Tốc độ di chuyển
public float health = 100f;         // Máu
public bool isMoving = true;       // Có di chuyển không
```

### Thay đổi spawn settings
Chỉnh sửa trong `AnimalSpawner.cs`:
```csharp
public float spawnInterval = 2f;    // Thời gian spawn
public int maxAnimals = 20;         // Số lượng tối đa
public float spawnRadius = 10f;     // Bán kính spawn
```

### Thêm logic tương tác
Thêm vào `Animal.cs`:
```csharp
private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        // Logic tương tác với player
    }
}
```

## Debug và Monitoring

### Debug UI
- Hiển thị số lượng animals active
- Buttons để control system
- System status

### Console Logs
- Thông tin về spawn/despawn
- Warnings và errors
- Performance metrics

## Performance Tips

1. **Object Pooling**: Luôn sử dụng object pooling cho animals
2. **Pool Size**: Điều chỉnh pool size phù hợp với game
3. **Spawn Rate**: Không spawn quá nhiều animals cùng lúc
4. **Cleanup**: Thường xuyên clear animals không cần thiết

## Troubleshooting

### Animals không spawn
- Kiểm tra AnimalPool có prefabs không
- Kiểm tra AnimalSpawner có active không
- Kiểm tra spawn area có hợp lệ không

### Performance issues
- Giảm maxAnimals
- Tăng spawnInterval
- Kiểm tra object pooling hoạt động đúng

### Sprites không hiển thị
- Kiểm tra sprite import đúng không
- Kiểm tra SpriteRenderer component
- Kiểm tra sorting order

## Mở rộng

### Thêm loại animal mới
1. Thêm vào `AnimalType` enum
2. Tạo prefab mới
3. Thêm vào AnimalPool
4. Cập nhật spawn logic

### Thêm behavior mới
1. Thêm methods vào `Animal.cs`
2. Override trong specific animal types
3. Thêm animation và effects

### Thêm AI behavior
1. Tạo AI script riêng
2. Attach vào Animal prefab
3. Implement pathfinding, decision making
4. Tích hợp với Animal system

## Liên hệ
Nếu có vấn đề hoặc cần hỗ trợ, vui lòng liên hệ team development.
