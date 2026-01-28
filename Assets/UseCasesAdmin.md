## Use Cases

### Use Case 1 Artikelverwaltung: Artikel erstellen

**Beschreibung:**  
Als Admin möchte ich neue Artikel erstellen, damit die Infrastruktur der Bibliothek verwaltet werden kann.

**Ablauf:**
1. Admin navigiert zu "Verwaltung".
2. Admin wählt die Untersektion "Artikelverwaltung" aus.
3. Admin gibt folgende Daten ein:
    - Name
    - Autor
    - Jahrgang
    - Beschreibung (optional)
    - Artikelart    
    - Aktuelle Menge von Autorenexemplaren
4. **Bedingung 1:** Wenn die Daten nicht korrekt sind, wird eine Fehlermeldung angezeigt.
5. **Bedingung 2:** Wenn die Daten korrekt sind, wird der Artikel im System gespeichert und die UI aktualisiert.

**Akzeptanzkriterien**
1. Der Artikel ist im System gespeichert.
2. Eine Fehlermeldung wird angezeigt, wenn die Eingaben ungültig sind.

---

### Use Case 1.2 Artikelverwaltung: Artikel aktualisieren

**Beschreibung:**  
Als Admin möchte ich bestehende Artikel bearbeiten können, damit die Infrastruktur der Bibliothek verwaltet werden kann.

**Ablauf:**
1. Admin navigiert zu "Verwaltung".
2. Admin wählt die Untersektion "Artikelverwaltung" aus.
3. Admin kann:
    - einen Artikel aus einer Liste auswählen,
    - die folgenden Daten aktualisieren:
        - Name
        - Autor
        - Jahrgang
        - Beschreibung
    - Änderungen speichern.

**Akzeptanzkriterien**
1. Die UI zeigt die aktualisierten Daten des Artikels an.

---

### Use Case 1.3 Artikelverwaltung: Artikel löschen

**Beschreibung:**  
Als Admin möchte ich bestehende Artikel löschen, damit die Infrastruktur der Bibliothek verwaltet werden kann.

**Vorbedingung:**  
Artikel dürfen nicht gelöscht werden, da sie für andere Use Cases (z. B. Use Case 4) benötigt werden und aus Gründen der Datensicherheit. Stattdessen werden sie intern als „deleted“ markiert.

**Ablauf:**
1. Admin navigiert zu "Verwaltung".
2. Admin wählt die Untersektion "Artikelverwaltung" aus.
3. Admin kann:
    - einen spezifischen Artikel auswählen,
    - die Löschung des ausgewählten Artikels bestätigen.
4. Das System aktualisiert die UI und zeigt gelöschte Artikel nicht mehr an.

**Akzeptanzkriterien**
1. Der ausgewählte Artikel ist im System als gelöscht markiert.

---

### Use Case 1.4 Artikelverwaltung: Neue Artikelexemplare einfügen

**Beschreibung:**  
Als Admin möchte ich die bestehenden Artikelexemplare erweitern, damit zusätzliche Exemplare zu spezifischen Artikeln hinzugefügt werden können.

**Ablauf:**
1. Admin navigiert zu "Verwaltung".
2. Admin wählt die Untersektion "Artikelverwaltung" aus.
3. Admin kann:
    - die gewünschte zusätzliche Menge von Artikelexemplaren eingeben.
4. **Bedingung 1** Pro Aktion dürfen nicht mehr als 10 Artikelexemplare hinzugefügt werden.

**Akzeptanzkriterien**
1. Die Anzahl der Artikelexemplare wird mit aktualisierten Daten in der UI angezeigt.
2. Die neuen Artikelexemplare können weiter ausgeliehen oder verbucht werden.

---

### Use Case 2 Benutzerverwaltung: Kundekonto sperren

**Beschreibung:**  
Als Admin möchte ich ein Kundenkonto sperren können, damit gesperrte Kundenkonten nicht mehr aktiv sind und sich der Kunde nicht einloggen kann.

**Ablauf:**
1. Admin navigiert zu "Verwaltung".
2. Admin wählt die Untersektion "Benutzerverwaltung" aus.
3. Admin kann:
    - ein spezifisches Kundenkonto auswählen,
    - das ausgewählte Kundenkonto sperren.
4. Das System aktualisiert die UI und zeigt das Kundenkonto als "Inaktiv" an.

**Akzeptanzkriterien**

1. Das Kundenkonto wird in der UI als "Inaktiv" angezeigt.
2. Der Kunde kann sich nicht einloggen.
3. Der Kunde erhält eine UI-Fehlermeldung beim Login Versuch.

---

### Use Case 2.1 Benutzerverwaltung: Kundekonto entsperren

**Beschreibung:**  
Als Admin möchte ich ein Kundenkonto entsperren können, damit in Sonderfällen benötigte Aktionen durchgeführt werden können.

**Ablauf:**

1. Admin navigiert zu "Verwaltung".
2. Admin wählt die Untersektion "Benutzerverwaltung" aus.
3. Admin kann:
    - ein spezifisches Kundenkonto auswählen,
    - das ausgewählte Kundenkonto entsperren.
4. Das System aktualisiert die UI und zeigt das Kundenkonto als "Aktiv" an.

**Akzeptanzkriterien**
1. Der Admin kann das Kundenkonto entsperren.
2. Das Kundenkonto wird in der UI als "Aktiv" angezeigt.

---

### Use Case 3 Richtlinienverwaltung: Richtlinie erstellen

**Beschreibung:**  
Als Admin möchte ich eine neue Richtlinie erstellen, damit für jeden Benutzertyp und Artikeltyp eigene Regeln definiert werden können.

**Vorbedingungen:**  
Richtlinien müssen individuell für einen bestimmten Benutzertyp und einen bestimmten Artikeltyp definiert werden.

**Ablauf:**

1. Admin navigiert zu "Verwaltung".    
2. Admin wählt die Untersektion "Richtlinienverwaltung" aus.
3. Admin kann:
    - eine Richtlinie erstellen,
    - ein Formular mit folgenden Daten ausfüllen:
        - Richtlinienname
        - Benutzertyp (z. B. Teacher)
        - Artikeltyp (z. B. Book)
        - Ausleihdauer in Tagen
        - Ausleihgebühr
        - Maximale Anzahl von Ausleihverlängerungen            
    - die neu ausgefüllten Daten speichern.
4. Das System aktualisiert die UI und zeigt die neue Richtlinie an.

**Akzeptanzkriterien**
1. Die neue Richtlinie wird in der UI angezeigt.

---

### Use Case 3.1 Richtlinienverwaltung: Richtlinie löschen

**Beschreibung:**  
Als Admin möchte ich eine bestehende Richtlinie löschen, damit das System aufrechterhalten werden kann.

**Ablauf:**
1. Admin navigiert zu "Verwaltung"
2. Admin wählt "Richtlinienverwaltung" aus.
3. Admin kann:
    - eine Richtlinie auswählen,
    - die Löschung bestätigen.
4. **Bedingung 1:** Wenn bereits ein Artikel mit dieser Richtlinie ausgeliehen ist, darf sie nicht gelöscht oder deaktiviert werden.
5. **Bedingung 2:** Wenn ein Kunde versucht, einen neuen Artikel auszuleihen, wird eine Fehlermeldung angezeigt, dass es keine Richtlinie gibt.

**Akzeptanzkriterien**
1. Die Richtlinien werden in der UI aktualisiert angezeigt.

---

### Use Case 3.2 Richtlinienverwaltung: Richtlinie aktualisieren

**Beschreibung:**  
Als Admin möchte ich eine bestehende Richtlinie aktualisieren, damit das System aufrechterhalten werden kann.

**Ablauf:**
1. Admin navigiert zu "Verwaltung".
2. Admin wählt "Richtlinienverwaltung" aus.
3. Admin kann:
    - eine spezifische Richtlinie auswählen,
    - folgende Daten aktualisieren:
        - Ausleihdauer in Tagen            
        - Ausleihgebühr
        - Maximale Anzahl von Ausleihverlängerungen
    
4. Das System speichert die aktualisierten Daten und verwendet sie für zukünftige Aktionen, z. B:
    - Verlängerung der Ausleihdauer.

**Akzeptanzkriterien**
1. Die Richtlinien werden in der UI aktualisiert angezeigt.
2. Der Kunde merkt die aktualisierte Richtlinie indirekt.

---

### Use Case 4 Benutzerverwaltung: Ausleihhistorie einsehen

**Beschreibung:**  
Als Admin möchte ich die vollständige Ausleihhistorie eines Kunden einsehen können, um einen Überblick zu erhalten.

**Ablauf:**
1. Admin navigiert zu "Verwaltung".    
2. Admin wählt "Benutzerverwaltung" aus.
3. Admin kann:
    - ein gewünschtes Kundenkonto auswählen,
    - die Ausleihhistorie des Kunden durchblättern.

**Akzeptanzkriterien**
1. Das System zeigt eine Ausleihhistorienliste an, die aktive und inaktive Ausleihen beinhaltet.

