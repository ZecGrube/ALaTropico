# Military & Invasion System Design

## Цель
Добавить внешний военный контур: развитие армии, бюджетное содержание, риск вторжений и варианты реакции игрока через дипломатию и союзы.

## Основные сущности

### MilitaryManager (singleton)
**Ответственность:** единый источник истины по военной мощности острова.

**Состояние:**
- `armyStrength` — суммарная численность/боевая масса.
- `trainingLevel` (0–100) — уровень подготовки.
- `armyLoyalty` (0–100) — политическая лояльность военных.
- `coastalDefenseLevel` — снижение урона морского вторжения.
- `airPowerLevel` — задел для будущих авиа-механик.
- `foreignSupportStrength` — временная поддержка союзников.

**Ключевые методы:**
- `AddArmyStrength/RemoveArmyStrength`
- `ImproveTraining/ReduceTraining`
- `AddCoastalDefense/RemoveCoastalDefense`
- `AddAirPower/RemoveAirPower`
- `CalculateDefensePower()`
- `CalculateMonthlyUpkeep()`
- `ApplyMonthlyBudgetPressure()`

### Военные здания
- **Barracks** — даёт прирост `armyStrength`, небольшой monthly upkeep.
- **MilitaryBase** — повышает `trainingLevel`, может повышать `foreignSupportStrength` при активных союзах.
- **CoastalDefense** — повышает `coastalDefenseLevel`.
- **Airfield** — повышает `airPowerLevel`.

## Вторжения

### Invasion (runtime event)
**Параметры:**
- `invader`
- `invasionStrength`
- `target`
- `durationMonths`
- `isNaval`
- `reparationsCost`

### Триггер
- Шанс вторжения проверяется раз в месяц в `EventManager`.
- Базовый шанс 5%.
- Увеличивается при низких отношениях и отсутствии союзов.
- Снижается при высокой обороне и активных союзах.

### Выборы игрока
- **Resist**: сравнение `CalculateDefensePower()` vs `invasionStrength`.
- **Negotiate**: потеря денег/мандата, но меньше разрушений.
- **Request Aid**: запрашивает помощь у `GlobalMapManager` (через союзы), затем повторный расчёт обороны.

### Последствия
- При поражении: штраф к легитимности, потери в казне, урон зданиям (особенно порт/культура), снижение лояльности.
- При победе: рост легитимности и лояльности армии.

## Интеграции
- **EconomyManager**: ежемесячно списывает `militaryUpkeep`.
- **GlobalMapManager**: даёт силу военной помощи от союзников.
- **SaveSystem**: сохраняет параметры армии и активное вторжение.
- **UI**: отдельная панель вторжения + индикатор силы армии.

## Баланс
- Целевая частота вторжений: ~1 событие в 3–5 игровых лет при нейтральной политике.
- Контр-игра: инвестиции в армию/альянсы должны заметно снижать риск, но не обнулять его.
