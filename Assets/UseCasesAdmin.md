## Use Cases

### Use Case 1: Artikelverwaltung Einfügung

**Beschreibung:** Als Admin möchte ich neue Artikel hinzufügen, damit die Infrastruktur der Bibliothek verwaltet werden kann.

**Ablauf:**
1. Admin geht zu „Verwaltung“.
2. Admin wählt „Artikelverwaltung“.
3. Admin macht:
    - Buch hinzufügen → Formular ausfüllen → speichern

**Akzeptanzkriterien**

1. Die Bücher werden im System gespeichert.

---

### Use Case 1.2: Artikelverwaltung Aktualisierung

**Beschreibung:** Als Admin möchte ich bestehende Artikel bearbeiten, damit die Infrastruktur der Bibliothek verwaltet werden kann.

**Ablauf:**
1. Admin geht zu „Verwaltung“.
2. Admin wählt „Artikelverwaltung“.
3. Admin macht:
    - Buch auswählen → Aktualisierung durchführen → Änderungen speichern
        

**Akzeptanzkriterien**
1. Der ausgewählte Artikel wurde aktualisiert.

---

### Use Case 1.3: Artikelverwaltung Löschung

**Beschreibung:** Als Admin möchte ich bestehende Artikel komplett löschen, damit die Infrastruktur der Bibliothek verwaltet werden kann.

**Ablauf:**
- Buch auswählen → Löschung bestätigen
**Akzeptanzkriterien**
1. Der ausgewählte Artikel wurde aus dem System entfernt.

---

### Use Case 1.4: Artikelverwaltung: Artikellöschung mit bestimmter Menge

**Beschreibung:** Als Admin möchte ich die bestehenden Artikelexemplare löschen, damit alle restlichen Exemplare intakt bleiben können.

**Ablauf:**
1. Admin wählt „Verwaltung“.
2. Admin kann:
    1. Die gewünschte Menge von Artikeln eingeben.

**Akzeptanzkriterien**
1. Die Artikel zeigen die aktualisierte Menge.    

---

### Use Case 1.5: Artikelverwaltung: Alle Artikel löschen, während Kunde sie als „Ausgeliehen“ hat

**Beschreibung:** Als Admin möchte ich alle Artikel löschen lassen, aber wenn jemand diese Artikel in Besitz hat, sei es als „Ausgeliehen“, dann sollen sie freigegeben werden, damit keine weiteren Vorgehen vorgenommen werden müssen.

**Akzeptanzkriterien**

1. Der Kunde kann die Artikel nicht an die Bibliothek zurückgeben.
2. Falls die gelöschten Artikel noch im Besitz sind, soll die Person benachrichtigt werden, dass sie diese nicht mehr retournieren kann.

---

### Use Case 2: Benutzerverwaltung

**Beschreibung:** Als Admin möchte ich das Kundenkonto sperren können, damit in Sonderfällen benötigte Aktionen durchgeführt werden können.

**Akzeptanzkriterien**
1. Der Admin kann das Kundenkonto sperren.    

---

### Use Case 2.1: Benutzerverwaltung

**Beschreibung:** Als Admin möchte ich Kunden löschen lassen, damit die Infrastruktur aufrechterhalten wird.

**Akzeptanzkriterien**
1. Der Admin kann bestehende Kunden aus dem System löschen.

---

### Use Case 2.2: Benutzerverwaltung

**Beschreibung:** Als Admin möchte ich die Kunden-Identitätsanfrage bestätigen lassen, damit Kunden für sich Konten eröffnen können.

**Akzeptanzkriterien**
1. Der Admin kann neue Kunden ins System einfügen.
2. Der Admin kann bestehende Kundeninformationen bearbeiten.

---

## Use Case 3: Richtlinien einsehen

**Beschreibung:** Als Admin möchte ich einen Überblick über alle Richtlinien haben, damit die Infrastruktur und Beziehungen zwischen Objekten und Subjekten korrekt definiert werden können.

**Ablauf:**
1. Admin geht zu „Verwaltung“.
2. Admin wählt „Richtlinienverwaltung“.
3. Admin kann:
    - Richtlinie manuell hinzufügen → Formular ausfüllen → speichern
    - Richtlinie aktualisieren → Änderungen speichern
    - Richtlinie auswählen → löschen

**Akzeptanzkriterien**
1. Richtlinien werden in der UI angezeigt.

---

### Use Case 3.1: Richtlinie erstellen

**Beschreibung:** Als Admin möchte ich eine neue Richtlinie erstellen, damit alle Benutzer ihre eigenen Regeln haben können.

**Ablauf:**
1. Admin geht zu „Verwaltung“.
2. Admin wählt „Richtlinienverwaltung“.
3. Admin kann:
    - Richtlinie auswählen → Formular ausfüllen → speichern        

**Akzeptanzkriterien**
1. Die neuen Richtlinien werden in der UI angezeigt.

---

### Use Case 3.2: Richtlinie löschen

**Beschreibung:** Als Admin möchte ich eine bestehende Richtlinie löschen, damit das System aufrechterhalten werden kann.

**Ablauf:**
1. Admin geht zu „Verwaltung“.
2. Admin wählt „Richtlinienverwaltung“.
3. Admin kann:
    - Richtlinie auswählen → Löschung bestätigen

**Akzeptanzkriterien**
1. Die Richtlinien werden in der UI aktualisiert angezeigt.

---

### Use Case 3.3: Richtlinie aktualisieren

**Beschreibung:** Als Admin möchte ich eine bestehende Richtlinie aktualisieren, damit das System aufrechterhalten werden kann.

**Ablauf:**
1. Admin geht zu „Verwaltung“.
2. Admin wählt „Richtlinienverwaltung“.
3. Admin kann:
    - Richtlinie auswählen → speichern

**Akzeptanzkriterien**
1. Die Richtlinien werden in der UI aktualisiert angezeigt.

---

### Use Case 4: Ausleihvorgänge überwachen

**Beschreibung:** Als Admin möchte ich die vollständige Ausleihhistorie eines Kunden einsehen können, um einen Überblick zu erhalten.

**Ablauf:**
1. Admin geht zu „Verwaltung“.
2. Admin wählt „Benutzerverwaltung“.
3. Admin kann:
    - Kunde auswählen → Ausleihhistorie des Kunden durchblättern.

**Akzeptanzkriterien**
1. Der Admin kann die gesamte Ausleihhistorie eines spezifischen Kunden einsehen.