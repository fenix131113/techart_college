const web_socket = require('ws')

const player_speed = 0.1
const minX = -9.825
const minZ = -9.765
const maxX = 9.594
const maxZ = 9.709
let registered_players = []
let player_coins = []
let spawned_coins = 0

const wss = new web_socket.Server({ port: 3000 }, () => { console.log("Server started!") })

wss.on("connection", (ws) => {

    ws.on("message", (data) => {
        onDataReceived(ws, data)
    })
})

wss.on("listening", () => {
    console.log("Server  listening on 3000")

    intervalId = setInterval(() => {
        spawnCoin()
    }, 3000);
})

function spawnCoin() {
    if (registered_players.length != 2)
        return

    let x = getRandomFloat(minX, maxX)
    let z = getRandomFloat(minZ, maxZ)

    spawned_coins += 1

    registered_players.forEach(element => {
        sendResponse(element.socket, "spawn_coin", [x.toString(), z.toString()])
    });
}

function collectCoin(socket, playerIndex) {
    spawned_coins -= 1
    player_coins[playerIndex] += 1
    sendResponse(socket, "display_coins", [player_coins[playerIndex].toString()])
}

/**
 * @param {import('ws').WebSocket} ws
 * @param {import('ws').RawData} data
 */
function onDataReceived(ws, data) {
    const message = data.toString()
    let loaded

    try {
        loaded = JSON.parse(message)
    } catch (e) {
        console.error("Invalid JSON:", message)
        return
    }

    const playerIndex = registered_players.findIndex(player => player.socket === ws);

    switch (loaded.commandString) {
        case "register_player":
            const register_entry = {
                id: registered_players.length,
                socket: ws
            };
            ws.playerId = register_entry.id
            ws.coins = 0
            registered_players.push(register_entry);
            player_coins.push(0)
            console.log(`Игрок ${register_entry.id} зарегистрирован`);

            const playerIds = registered_players.map(player => player.id);

            console.log(JSON.stringify(playerIds));
            const ids_obj = {
                ids: playerIds
            }

            registered_players.forEach(element => {
                sendResponse(element.socket, "register_result", [element.id, JSON.stringify(ids_obj)]);
            });
            break;

        case "move":
            const [x, y] = loaded.Payload
            registered_players.forEach(element => {
                sendResponse(element.socket, "move", [playerIndex, loaded.Payload[0] * player_speed, loaded.Payload[1] * player_speed])
            });
            break

        case "collect_coin":
            collectCoin(ws, playerIndex)
            console.log("Player " + playerIndex + " collected coin = " + player_coins[playerIndex])
            break

        default:
            console.warn("Неизвестная команда:", loaded.commandString)
    }
}

/**
 * @param {WebSocket} ws
 * @param {string} command
 * @param {object} payload
 */
function sendResponse(ws, command, payload) {
    const response = {
        commandString: command,
        Payload: payload
    };

    const msg = JSON.stringify(response);

    ws.send(msg);
}

function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function getRandomFloat(min, max) {
    return Math.random() * (max - min) + min;
}