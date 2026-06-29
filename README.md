# JuanCruzIzurietaIA
Trabajo de IA UADE 2026

Juego: El juego se llama "P&F battle for the tri state area" y se ambienta en una pelea relacionada a la serie Phineas y Ferb. Por eso la estetica de tejado y los 2 modelos. El objetivo del juego es escapar del doctor hasta conseguir un objeto que te permite derrotarlo (En este caso una gema, pero sera remplazada mas adelante), al hacerlo el doctor dejara de perseguirte y empezara a huir de ti.

Sistemas de ia implementados: Wander, Seek, Flee.

Controles basicos: WASD: Movimiento, Mouse: Camara.

Entrega 2:

Se añadio el segundo nivel, el primero se mantuvo igual, con el enemigo haciendo wander, seek y flee. Pero en el segundo se añaden 2 enemigos con IAs diferentes:

Doof pelota: Se mueve aleatoriamente entre puntos del laberinto, usando A*, si te ve en algun momento, te perseguira directamente con el seek. Si te pierde, volvera a hacer wander con A*
Doof monocromatico: Tambien explora el laberinto entre puntos aleatorios con A*. Pero este si te ve, se asustara. Elegira un punto del laberinto lo mas alejado posible de ti (Esto no es perfecto, lo que hace es buscar un punto lejos que este direccion opuesta a tu pj, pero como es un laberinto, a veces para ir a ese punto correra a ti, pero la mayoria del tiempo cumple) A su vez, alertara al Doof Pelota, el cual cambiara su nodo objetivo a cual sea tu nodo mas cercano. Por lo que si te encuentras a este, posiblemente el Doof pelota este viniendo hacia ti.

La forma de ganar es escapar del laberinto sin que te atrape el doof pelota. 
