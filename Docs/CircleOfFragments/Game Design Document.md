# 🌀 Círculo dos Fragmentos — Game Design Document (Visão)
### Documento de visão geral · destinado a dar o panorama do projeto

> **Como ler este documento.** Isto **não** é uma especificação técnica nem um plano fechado. É a visão do jogo — o que ele quer ser, como se joga, e onde a arte entra. Cada seção carrega uma etiqueta de confiança pra deixar claro o que já está decidido e o que ainda é ideia:
>
> - 🔒 **Travado** — decisão tomada, base sólida.
> - 🌓 **Provisório** — direção atual, mas aberta a mudar.
> - 💭 **Aspiracional** — ideia de futuro, ainda não decidida nem planejada.
>
> O nome do jogo em si ainda é provisório.

---

## 1. Pitch

> **Um RPG tático por turnos onde você monta um time de campeões mágicos e atravessa expedições para reunir os fragmentos de um mundo despedaçado.**

O combate é o coração: turnos dinâmicos baseados em velocidade (estilo *Honkai: Star Rail*), onde a ordem de ação muda o tempo todo e manipular essa ordem é parte da estratégia. Em volta dele, um loop de expedições que dá ritmo de "run" — você entra, escolhe sua rota, junta recursos e sai mais forte.

**Gênero:** RPG Tático por Turnos · Roguelite de expedição · Coleção de personagens
**Plataforma:** PC (foco inicial), com adaptação a mobile como possibilidade 🌓
**Pilares de design** 🌓
- **Combate tático com profundidade real** — velocidade, elementos, buffs e manipulação de turno se cruzam.
- **Time como expressão** — montar e evoluir o seu time é o investimento central do jogador.
- **Coleção sem pressão** — adquirir personagens é parte da graça, não uma parede de monetização (ver §6).

---

## 2. Mundo & Temática 🌓

> Lore base, provisória. Serve de ambientação e fio condutor — não é o foco do jogo, e nada aqui está fechado.

**A Guerra Arcana e os Fragmentos**

Houve uma guerra que partiu o mundo. Para vencê-la, os reinos recorreram ao **Círculo dos Fragmentos** — um portal arcano capaz de invocar heróis de outros mundos para lutar suas batalhas.

Mas o Círculo é mais antigo que qualquer entendimento que se tenha dele. Os reinos sabem _usá-lo_ — não sabem o que ele é. E ignoram que cada invocação rasga o véu da realidade, abrindo **fendas** por onde criaturas de outros mundos atravessam para o nosso.

Quando a guerra enfim cessou, o mundo que sobrou era um arquipélago de **reinos flutuantes** — cacos de um todo que já não existe. E as fendas permaneceram, sangrando monstros sobre as ruínas.

Para combatê-las, os reinos fazem a única coisa que conhecem: recorrem ao Círculo. Invocam mais heróis. Enviam-nos em **expedições** para selar as fendas, uma a uma — sem jamais suspeitar que é o próprio portal, sua única esperança, que as abre.

O tom pretendido é de **fantasia mágica** — o "como isso parece" está deliberadamente em aberto (ver §7). Os **Fragmentos** dão nome ao jogo e ambientam o mundo; por enquanto funcionam como recurso de gameplay, com espaço pra ganharem peso narrativo no futuro. 💭

---

## 3. Personagens 🌓

Unidades jogáveis são **campeões mágicos**, cada um com elemento, classe (arquétipo) e um kit próprio: ataque básico, habilidade especial, ultimate e uma passiva única.

- **Raven** 🌓 — a personagem-âncora, a única com conceito já formado. Serve de referência do que um campeão é no jogo.
- **Tokiri** — personagem **temporário de teste**, não faz parte do elenco planejado.
- **Elenco-alvo** 💭 — a ambição de longo prazo gira em torno de **20-30 personagens**. É meta, não compromisso: nada além de Raven está definido.

Cada personagem tem atributos divididos em **normais** (vida, ataque, defesa, velocidade — cálculos mais ricos, com modificadores) e **especiais** (mana, crítico, bônus de dano, etc. — cálculos mais diretos). A intenção é que raridade, elemento e classe alimentem **sinergias de time** no futuro. 💭

---

## 4. Como se joga

### 4.1 Loop macro — a jornada do jogador 🌓

O ciclo central que mantém o jogador engajado:

1. **Expedição** → mergulha numa run e junta **Cristais** (a moeda principal) e recursos de upgrade.
2. **Adquirir personagem** → usa os Cristais pra trazer um novo campeão ao time (ver §6).
3. **Farmar / Auto-farmar** → roda os modos de farm pra juntar os recursos que sobem o nível dos personagens.
4. **Evoluir** → upa o novo campeão, ou reforça um que já tem.
5. **De volta à expedição**, agora mais forte, pra encarar desafios maiores.

> **Nota:** farm e auto-farming **fazem parte do loop central**, não são extra — são a fonte primária de recursos de evolução. Sem eles o ciclo não fecha. (O auto-farming é o que tira o tédio da repetição: você libera o conteúdo jogando, depois deixa ele rodar sozinho pra acumular.)

### 4.2 A Expedição 🌓

A expedição é a principal fonte de Cristais e o lado "roguelite" do jogo. A estrutura imaginada:

- **Não é mundo aberto.** É um **tabuleiro de casas** — você avança escolhendo sua **rota** entre caminhos possíveis.
- Cada casa tem um tipo com efeito próprio: **combate, recuperação, armadilha, recompensa, neutro** — e outros a definir.
- A escolha de rota é a decisão estratégica da camada macro: arriscar uma armadilha por uma recompensa melhor, ou jogar seguro pela recuperação.

O jogador entra na expedição com um **time fixo de 4 personagens**, montado antes de partir. 🌓
Uma ideia em estudo é uma **casa de recrutamento** que deixa adquirir personagens *durante* a run — interessante sobretudo em níveis mais altos — mas isso ainda não está decidido. 💭

**O que persiste ao fim de uma run:** os **Cristais** acumulados e **alguns recursos de upgrade**. A evolução de personagem mora no **meta** (fora da run), alimentada principalmente pelos modos de farm — não pela expedição. 🌓

**Expedição como endgame** 🌓 — a direção atual é que a própria expedição seja o endgame do jogo, na forma de uma **Expedição Infinita**: o mesmo tabuleiro, mas sem fim definido, ficando mais difícil a cada nível avançado e recompensando conforme você chega mais longe. O conceito está esboçado na §9, ainda a lapidar.

### 4.3 Loop micro — o Combate 🔒

O combate é a parte mais madura do projeto (é o que está sendo reconstruído agora — ver §8). A sensação-alvo:

- **Fila de turnos dinâmica por Action Value (AV):** a ordem de quem age é calculada por velocidade e muda continuamente. Avançar ou atrasar o turno de alguém é uma ferramenta tática de primeira classe.
- **Times de até 4 jogadores** contra **até 5 inimigos** 🔒, em categorias **Comum, Elite e Boss**. 🌓
- **Mana como recurso** das habilidades — algumas gastam, outras devolvem.
- **Três ações por personagem:** básico, habilidade e ultimate.
- **Sistema elemental** com vantagens, fraquezas e resistências, que mexem em dano, crítico e chance de efeito.
- **Habilidades modulares:** cada skill combina efeitos (dano, cura, buff, debuff, manipulação de turno) sobre alvos variados (único, área, aliado, inimigo).

A leitura de combate que a arte precisa sustentar está detalhada na §7.

---

## 5. Progressão de personagem 🌓

Fora da run, o jogador investe nos campeões:

- **Subida de nível** com recursos farmados, até um teto alto (o GDD antigo cita **nível 150** 💭 — número provisório).
- **Ascensões** que destravam tetos e reforçam atributos.
- **Modificadores** (base, percentual, flat) que sustentam buffs, debuffs e — no futuro — equipamentos.
- **Raridades** (S e SS) como camada de coleção e poder. 🌓

---

## 6. Aquisição de Personagens — gacha sem coleira 🌓

Esta é uma decisão de **identidade**, não só de mecânica. A intenção é ter um gacha **que não seja limitante nem predatório** — explicitamente **anti-microtransação**.

Como funciona a ideia:

- Existe **uma moeda só** — os **Cristais**, ganhos jogando (a expedição é a fonte principal). **Sem moeda premium separada.**
- O **gacha** existe pra quem quer **apostar na sorte**: tem a chance de tirar um personagem gastando menos.
- A **loja** vende o mesmo personagem por um preço fixo equivalente ao **pior caso** do gacha. Quem não quer depender da sorte simplesmente **compra** com o que juntou jogando.

> **Exemplo ilustrativo** (números 100% especulativos): se no pior caso o gacha levaria ~120 tiros a 100 Cristais cada pra garantir um personagem, a loja lista esse personagem por **12.000 Cristais**. O gacha então é só a chance de pagar menos — nunca a única porta.
>
> Resultado: o jogador **sempre** consegue o que quer jogando. A sorte é um bônus, não um pedágio.

Há inclusive a possibilidade de o jogo **não ser gacha** no fim — essa estrutura é a direção atual, não um compromisso. 💭

---

## 7. Identidade Visual — o espaço da artista 🌓

> Esta seção é **deliberadamente aberta**. O estilo de arte é **100% escolha da artista**. O que segue **não** diz como o jogo deve parecer — diz só o que o jogo precisa que a arte **comunique** pra funcionar. O "como" é dela.

O GDD antigo registra uma intenção de **pixel art detalhada de fantasia mágica medieval** — mas trate isso como **uma** possibilidade entre muitas, não como direção fechada. A liberdade criativa é intencional.

**O que o combate precisa que a arte deixe legível:**

- **Estado de cada unidade num relance** — viva/morta, vida e mana, quem está buffado/debuffado.
- **De quem é o turno** e qual a ordem da fila — o AV é o coração tático, então "quem age agora e depois" tem que ler imediato.
- **Identidade de cada personagem** — elemento, classe e personalidade visível o bastante pra o jogador se apegar e reconhecer.
- **Peso das ações** — básico, habilidade e ultimate precisam *sentir* diferentes; a ultimate é a vitrine.
- **Leitura de alvo** — quem pode ser alvo de quê, sem ambiguidade.
- **Reação a eventos** — dano, cura, crítico, fraqueza, morte: feedback que o jogador sente.

Dentro disso, o estilo, a paleta, o partido visual e o tom são território criativo aberto.

---

## 8. Onde estamos hoje 🔒

O projeto tem uma **base de combate já implementada** e está em **refactor da arquitetura de combate** — reconstruindo as fundações pra ficarem limpas e extensíveis antes de seguir.

O foco imediato é um **vertical slice**: **uma única luta, bonita, em 3D, do começo ao fim** — 1 personagem totalmente animado contra 2 inimigos, usando básico/habilidade/ultimate, com um buff de demonstração, terminando em vitória. O objetivo do slice é **provar o pipeline arte ↔ código e a sinergia da dupla** — não avançar o conteúdo do jogo.

> Em resumo: a ambição deste documento é grande, mas o passo atual é pequeno e focado de propósito. Construir uma luta excelente primeiro, expandir depois.

---

## 9. Expansões Futuras 💭

> Tudo nesta seção é **ideia de futuro** — conhecida e desejada, mas **não decidida nem planejada**. Está aqui pra mostrar a ambição do projeto, não um cronograma.

- **Casa de recrutamento na expedição** — adquirir personagens *durante* uma run, relevante em níveis altos.
- **Expedição Infinita (endgame)** — mesmo tabuleiro da expedição, mas **infinito** (ou com teto altíssimo, na casa dos ~1000 níveis). A cada nível avançado a dificuldade sobe, e as recompensas escalam conforme você chega mais longe. É a direção atual pro endgame do jogo. *(Conceito inicial, a lapidar.)*
- **Forja de Artefatos** — onde você **recicla** artefatos antigos em recursos e **melhora** os que já possui (não é a fonte de obtenção dos artefatos em si). *(Conceito inicial, a lapidar.)*
- **Sinergias de classe** — bônus por composição de time, ligando classe/elemento/raridade à estratégia.
- **Passivas avançadas** — de inimigos, e globais persistentes entre etapas.
- **IA estratégica** — inimigos que leem buffs, ameaça e a fila de turnos, em vez de mirar aleatório.
- **Equipamento & Ascensão aprofundados** — camadas extras de build e poder.

*(Me passa qualquer outro modo que você já tenha na cabeça e eu encaixo aqui.)*

---

## 10. Glossário de confiança

| Etiqueta | Significado | Exemplos neste doc |
|---|---|---|
| 🔒 **Travado** | Decidido, base sólida | Combate por AV; foco no vertical slice atual |
| 🌓 **Provisório** | Direção atual, pode mudar | Loop de expedição, modelo de gacha, lore, Raven |
| 💭 **Aspiracional** | Ideia de futuro, não decidida | Elenco de 20-30, expedição infinita, forja, nível 150, recrutamento na run |

---

*Documento de visão · vivo, feito pra evoluir conforme as decisões fecham.*
