# Circle of Fragments

A tactical turn-based RPG prototype developed in Unity.

## Overview

Circle of Fragments is a tactical RPG focused on deep combat mechanics,
dynamic turn order, character building and modular skill systems.

The project is being developed as a personal project while studying
Computer Science, with the goal of exploring game architecture and combat systems.

## Core Systems

The project currently includes the following gameplay systems:

- Dynamic Turn Based Rpg System
- Passive System
- Wave System
- Modular Skill Effect System
- Modifier / Buff System
- Custom Character Editor

## Architecture

The combat system follows an event-driven architecture separating
responsibilities across multiple managers such as:

- GameManager
- TurnManager
- CombatManager
- WaveManager

This approach allows modular gameplay systems and easier scalability.

## Technologies

- Unity
- C#
- ScriptableObjects
- Event-driven architecture
- Singletons

## Project Status

Prototype currently focused on combat systems and gameplay architecture.
