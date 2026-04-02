# piloK

## Opis
Projekt roguelike napisany w Unity (C#).

Główna idea:
Gra oparta na falach przeciwników, z systemem przedmiotów i statystyk wpływających na rozgrywkę.

## Gdzie warto zajrzeć

---

### 1. Damageable system

- [[Damageable.cs](https://github.com/want-pizza/piloK/blob/main/Assets/Scripts/DamageSystem/Damageable.cs)]
- [[FloterDamageable.cs](https://github.com/want-pizza/piloK/blob/main/Assets/Scripts/Enemies/Floter/FloterDamageable.cs)]

Tutaj starałem się zrobić bardziej uniwersalne rozwiązanie do obsługi obrażeń.

---

### 2. Item / Stats system

- [[BaseItemObject.cs](https://github.com/want-pizza/piloK/blob/main/Assets/Scripts/Items/BaseItemObject.cs)]
- [[RuntimeItemData.cs](https://github.com/want-pizza/piloK/blob/main/Assets/Scripts/Items/RuntimeItemData.cs)]
- [[ItemStatData.cs](https://github.com/want-pizza/piloK/blob/main/Assets/Scripts/Items/ItemStatObject.cs)]

---

### 3. Inventory system

- [[InventoryObject.cs](https://github.com/want-pizza/piloK/blob/main/Assets/ScriptableObjects/Inventory/InventoryObject.cs)]
- [[InventoryPresenterBase.cs](https://github.com/want-pizza/piloK/blob/main/Assets/Scripts/Inventory/InventoryPresenterBase.cs)]
- [[DisplayInventory.cs](https://github.com/want-pizza/piloK/blob/main/Assets/Scripts/Inventory/DisplayInventory.cs)]

Niektóre elementy UI (np. menu wyboru przedmiotów, timer fal) nie mają osobnej warstwy modelu, tylko kontroler i view — jestem ciekaw, czy takie podejście jest uznawane za poprawne w większych projektach.

### 4. Movement / Player state machine

Niektóre kwestie związane z [[ruchem gracza](https://github.com/want-pizza/piloK/blob/main/Assets/Scripts/Player/PlayerMovement.cs)] i [[state machine](https://github.com/want-pizza/piloK/blob/main/Assets/Scripts/Player/PlayerStateMachine.cs)] nadal są dla mnie wątpliwe.  
Nie jestem pewien, czy obecny sposób organizacji logiki movement jest optymalny i czy warto wprowadzać zmiany.  
Chętnie poznałbym opinię, czy takie podejście ma sens, czy lepiej byłoby inaczej rozdzielić odpowiedzialności.


## Dodatkowo

Repozytorium jest dość duże, więc wybrałem kilka miejsc do przejrzenia 🙂
