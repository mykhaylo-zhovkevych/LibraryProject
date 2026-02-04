## Ziel der Software

- **Ziel:** Eine Bibliotheks-App, die sowohl für Administration als auch für Kunden nutzbar ist.
- **Plattformen:** Desktop-App (Avalonia, MVVM) und Web-App (Blazor).
- **Datenhaltung:** lokal gespeichert, ggf. persistent (z.B. Datei oder SQLite).
- **Kernlogik**: Alle Bibliotheksfunktionen in einem **Core-Projekt** definiert und unabhängig von UI.

---
## Voraussetzungen

1. Die bestehende Codebasis für dieses Projekt übernehmen und drauf aufbauen.
2. Das System muss zwei Interfaces haben: eines für die Bibliotheksadministration und eines für Kunden.
3. Die Daten können lokal gespeichert werden; optional soll eine persistente Datenspeicherung umgesetzt werden.
4. Der Code muss statisch typisiert sein und den Clean-Code-Konventionen folgen.

---

## Benutzergruppen & Rollen

Admin
- Artikeln hinzufügen, bearbeiten, löschen
- Benutzer verwalten (registrieren, sperren, reaktivieren)
- Ausleihhistorie überwachen

Kunde
- Artikeln ausleihen, zurückgeben, reservieren
- Artikel suchen (mit Erweiterungen)
- Eigenes Konto verwalten (Konto schliessen)
- Ausleihhistorie einsehen (mit Erweiterungen)

---

## Use Cases
_Einige Use Cases können aus einer Benutzergruppen  zu andere Benutzergruppen mitgenommen und implementiert werden._ 
- [Admin Use Cases](./Assets/UseCasesAdmin.md)
- [Customer Use Cases](./Assets/UseCasesCustomer.md)
---

## Arbeitsplan und Arbeitspakete mit Aufwandsschätzungen

| Datum                   | Arbeitspaket                        | Erwartete Aufgabe                           | Erledigte Aufgabe                                         |
| ----------------------- | ----------------------------------- | ------------------------------------------- | --------------------------------------------------------- |
| 05.01.2026 / 06.01.2026 | Vorbereitung und Konzept erstellung | Das fertig Konzept                          | Das Konzept wurden während Projekt Ausführung nachgefasst |
| 07.01.2026 / 09.01.2026 | Projektinitialisierung              | Die Core Projekt und allfällige Remastering | Ähnlich wie in Arbeitspaket 1 aber in geringem Masse      |
| 12.01.2026 / 26.01.2026 | Desktop App Implementierung         | Das minimalistisch UI                       | Ja                                                        |
| 26.01.2026 / 03.02.2026 | Web App Implementierung             | Das minimalistisch UI und RWD               | Ja                                                        |

---
## Architekturkonzept

### Clean Architecture

[![Design Application using Clean Architecture - DEV Community](https://media2.dev.to/dynamic/image/width=1000,height=420,fit=cover,gravity=auto,format=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Forahufsvjymejio3beu7.png)

**Beschreibung** Das gesamte Projekt ist in vier Sichten (Layer) unterteilt. Jede Sicht hat eine klar definierte Verantwortung und darf nicht direkt mit anderen Sichten kommunizieren, sondern nur horizontal über definierte Schnittstellen.
Dies ermöglicht eine bessere Testbarkeit, Wartbarkeit und Erweiterbarkeit der Anwendung.

1. Domain Layer
2. Application Layer
3. Presentation Layer
4. Infrastructure Layer

**Vor -und Nachteile**

1. Klare Trennung der Verantwortlichkeiten
   Jede Schicht hat eine eindeutige Aufgabe: Die Domain ist für Geschäftslogik und Entitäten zuständig. Der Application Layer steuert Use Cases und den Ablauf der Anwendung. Die Infrastructure kümmert sich um Datenhaltung, Datenbanken und externe Services. Die Presentation ist für die Benutzeroberfläche verantwortlich.
2. Einfache Testbarkeit
   Unit-Tests können einfach umgesetzt werden, da die Infrastructure-Schicht durch Mock- oder Fake Implementierungen ersetzt werden kann.
3. Wartbarkeit
   Neue Features können hinzugefügt werden, ohne andere Layer stark zu verändern.
4. Saubere Abhängigkeiten
   Die Domain kennt die Infrastructure nicht, und die Application kennt die UI nicht.
   Dadurch entsteht eine lose Kopplung zwischen den einzelnen Teilen des Codes.

5. Setup Komplexität
   Der initiale Aufbau benötigt mehr Zeit, insbesondere bei kleinen oder mittelgrossen Projekten.
6. Event Hadling / Cross-Cutting Concerns
   Querschnittsthemen wie Logging, Validierung oder Security sind komplexer umzusetzen.

**Warum diese Architektur wurden ausgewählt?**
Ich habe diese Architektur ausgewählt, weil das passt für die kleine bis mitlernen Projekten die eine gemeinsamen Geschäftslogik haben müssen und die Trennung zwischen Sichten ermöglichen flexiblem Erwartbarkeit ohne die Kernlogik zu verändern müssen.

---
## Datenkonzept und High Level Design

## UX Design Prototype

https://drift-shed-51908381.figma.site


## Web App

Für diesen Teil habe ich Blazor Server mit Cookie-Authentifizierung gewählt, da die Anwendung konzeptionell einer Desktop-Applikation mit zentraler Shell und mehreren funktionalen Ansichten/Pages wie Katalog, Profil usw. entspricht. Blazor Server ermöglicht eine komponentenbasierte Architektur, bei der Zustände und Logik serverseitig in C# gehalten und UI-Interaktionen ohne vollständige Seitenneuladung verarbeitet werden können. Die Cookie-Authentifizierung stellt sicher, dass der Anmeldezustand persistent bleibt, auch wenn die Seite neu geladen wird.
Im Vergleich dazu basiert Razor Pages auf einem klassischen Request-Response-Modell, das für formularbasierte Webseiten geeignet ist, jedoch komplexe Navigationen und dynamische Zustandswechsel nicht so gut unterstützt.

| **Technologie**   | **Ausführung** | **Verbindungstyp**       | **Bester Anwendungsfall**                          |
| ----------------- | -------------------- | ------------------------ | -------------------------------------------------- |
| **Razor Pages**   | Server               | HTTP (Stateless)         | Einfache Webseiten, SEO-lastige Inhalte, Blogs.    |
| **Blazor Server** | Server               | **SignalR** (Persistent) | Echtzeit-Dashboards, interne Geschäftsanwendungen. |
| **Blazor WASM**   | **Browser**          | Keine (Standalone)       | Offline-fähige Apps, hohe Interaktivität.    |
| **Blazor Hybrid** | **Native App**       | Local Interop            | Desktop (WPF/MAUI) oder Mobile Apps mit Web-UI.    |
| **Blazor Auto**   | **Beides**           | Dynamisch | Moderne High-End Web-Apps.   |

                       |

## Schlussfolgerung und Ausblick

### Ergebniss

[Bildern und Design](./Assets/Screenshots.md)

Das Ziel dieses Projekts war es, Avalonia und Blazor zu lernen. Als erste Referenz habe ich die St. Galler Bibliothek als Vorlage genommen.

Ich habe geplant, das Projekt in drei Sprints umzusetzen:
Im ersten Sprint ging es um die Architekturwahl, im zweiten Sprint um zwei separate Anwendungen – eine Desktop-App mit Avalonia und eine Web-App mit Blazor. Zwischen den Sprints wollte ich jeweils Feedback einholen, damit neue Anforderungen der Stakeholder berücksichtigt werden können.

Während des zweiten Sprints gab es von den Stakeholdern den Wunsch, dass die Desktop-App nur für die Administration und die Web-App für die Kunden gedacht ist. Zusätzlich sollte der Registrierungs und Login-Prozess vereinfacht und einige Features angepasst werden.
Dank der gewählten Architektur musste ich dafür nicht sehr viel ändern. Im dritten Sprint konnte ich viele bereits vorhandene Methoden weiterverwenden. Trotzdem musste ich wegen der neuen Anforderungen die Application und Infrastructure-Schicht erweitern, damit die Web-Anforderungen vollständig erfüllt werden.
Als nächsten Schritt könnte ich noch Unit-Tests oder Integrationstests hinzufügen. Das war jedoch keine strikte Anforderung, deshalb habe ich es nicht umgesetzt.

Wenn ich alles zusammenfasse, habe ich in diesem Projekt viele neue und spannende Features von Avalonia und Blazor gelernt.