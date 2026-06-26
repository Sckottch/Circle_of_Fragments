# 🎯 Contrato de Escopo — Vertical Slice (Fase 2)
### Círculo dos Fragmentos — Piloto com a prima
**Status:** RASCUNHO v0 · sujeito a revisão conjunta
**Regra de ouro:** qualquer item fora do *Escopo IN* exige reabrir este contrato explicitamente. "Já que o código tá aqui" **não** é justificativa válida.

---

## 1. Objetivo / Definição de Pronto

> **Uma luta bonita que funciona, em 3D, do começo ao fim.**

Um único combate jogável: o jogador controla 1 personagem totalmente animado, enfrenta inimigos numa arena simples, usa básico/skill/ultimate, e vence. Nada além disso. O objetivo é testar a sinergia da dupla e o pipeline de assets — **não** avançar o jogo.

---

## 2. Escopo IN (o que entra)

- 1 personagem jogável, modelo 3D totalmente animado
- 2 inimigos (set de animação reduzido)
- 1 wave única (= 1 combate)
- Loop de turno completo por **Action Value**: ordem dinâmica, jogador escolhe ação, seleção de alvo, skill resolve, turno passa, inimigo age, repete
- 3 ações do jogador, cada uma com sua animação: **básico**, **skill**, **ultimate**
- **1 buff/debuff** no showcase — usando **só efeitos já existentes** (`DamageEffect` + `StatBuffEffect`)
- UI de combate existente: barra de ordem de turno, painel de ação, HP/mana, números de dano
- Condição de vitória (todos inimigos mortos) → estado/tela de fim
- Arena 3D minimalista + câmera fixa

---

## 3. Escopo OUT (o que NÃO entra) — o muro anti-creep

- ❌ Passivas — de player, de inimigo, globais. **Nenhuma.** (O stub existe no código; fica desligado no slice.)
- ❌ Tipos de SkillEffect novos (só os 2 que já existem)
- ❌ Ascensão, level up, equipamento, inventário
- ❌ Múltiplos personagens, party building, troca de personagem
- ❌ Múltiplas waves, progressão de wave, mapa de expedição
- ❌ Sinergia de classe
- ❌ IA além do stub atual (inimigo: alvo aleatório + ataque básico)
- ❌ Save/load, localização, polish de áudio/música
- ❌ Expansão das ferramentas de editor

---

## 4. Lista de assets funcionais (para a prima)

> **O estilo de arte é 100% escolha dela.** Esta lista é só sobre *quais beats de animação* o combate precisa para ler bem — não sobre como devem parecer.

**Personagem jogável:**
- Idle (loop)
- Ataque básico
- Skill especial
- Ultimate (a vitrine)
- Reação a dano (hit)
- Morte
- (opcional) Pose de vitória

**Inimigo (set reduzido):**
- Idle (loop)
- Ataque
- Hit
- Morte

**Cenário:** 1 arena simples. Pode ser minimalista.

---

## 5. Contrato de integração Arte ↔ Código

> Preencher **junto com ela** no início. É aqui que mora o aprendizado de pipeline. Não prescrever antes da conversa.

Pontos a definir em conjunto:
- Formato de export (provável FBX)
- Tipo de rig: **Humanoid (Mecanim)** vs Generic — impacta reuso de clips
- Convenção de nomes dos clips (ex.: `Hero_Idle`, `Hero_BasicAttack`, `Hero_Ultimate`)
- Escala e orientação de referência (convenção: 1 unidade Unity = 1 metro)
- **Hit frames:** em que frame de cada animação o dano "acerta" — pra sincronizar lógica com animação via Animation Events. (Esse é o ponto técnico mais valioso do pipeline.)

---

## 6. Superfície técnica → o que o REFACTOR precisa suportar

O slice é o que define os requisitos da Fase 1. Para entregar o que está no Escopo IN, o refactor **precisa**:

- `CombatContext` como **dono único** da lista de unidades (1 player + 2 inimigos)
- Loop de turno AV lendo do `CombatContext` (não de listas privadas de cada manager)
- Execução de skill pela pipeline de `SkillEffect` existente
- 1 buff aplicado / tickado / expirado via `BuffSystem` consultando o `CombatContext`
- Detecção de vitória num **único lugar**
- Teardown limpo no fim do combate
- Separação `Unit` (lógica) ↔ `UnitView` (visual): remover `[RequireComponent(typeof(SpriteRenderer))]`, trocar swap de sprite por `Animator`

---

## 7. O que o refactor NÃO precisa suportar ainda

Para o refactor não virar over-engineering:

- Persistência entre waves → o escopo de vida "persistente" é **definido como estrutura, mas fica vazio**. Slice é wave única.
- Passivas (qualquer tipo)
- Sinergia de classe, ascensão, equipamento
- IA estratégica

---

## 8. Critérios de aceitação (testáveis)

- [ ] Uma luta completa roda do início ao fim sem erro no console
- [ ] Ordem de turno por AV reflete as velocidades corretamente na UI
- [ ] Jogador usa básico, skill e ultimate — cada um com sua animação tocando
- [ ] 1 buff/debuff aplica, dura N turnos e expira corretamente
- [ ] Inimigo age (stub) no turno dele
- [ ] Vitória detectada quando inimigos morrem → estado de fim
- [ ] Nenhum sistema mantém lista própria de unidades — tudo via `CombatContext`
- [ ] Personagem é modelo 3D animado; nenhum `SpriteRenderer` no fluxo de combate

---

## 9. Decisões abertas (precisam da tua escolha)

- **D1 — Personagem do slice:** reusar Tokiri/Raven, ou a prima cria/desenha um novo?
  *Recomendação:* ela escolhe/cria, pra ter buy-in criativo — desde que o kit seja simples.
- **D2 — Quantidade de inimigos:** 1 ou 2?
  *Recomendação:* 2. Mostra seleção de alvo / AoE sem custo grande, já que o set de inimigo é enxuto.
- **D3 — Buff no showcase:** incluir 1, ou só dano?
  *Recomendação:* incluir 1 (já-existente). Prova o sistema modular.

---

## 10. A armadilha (ler antes de cada sessão)

O perigo único desta rota é a tentação: o código inteiro já está lá te chamando pra "ligar só mais um sistema". Toda adição fora do Escopo IN reabre este contrato. O contrato existe pra matar essa tentação — não pra ser conveniente.
