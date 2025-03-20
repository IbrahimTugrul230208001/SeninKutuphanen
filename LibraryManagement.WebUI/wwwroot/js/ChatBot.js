var hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7254/ai-hub")
    .build();

hubConnection.start()
    .then(() =>
        document.getElementById("connectionId").innerHTML = `(${hubConnection.connectionId})`)
    .catch(err =>
        console.error(err.toString()));

hubConnection.on("ReceiveMessage", responseMessage => {
    const aiMessage = `<div class="flex items-start">
                <img src="/img/logo.png" alt="Bot" class="w-10 h-10 rounded-full mr-3">
                <div id="chat-box" class="bg-gray-100 p-3 rounded-lg max-w-xl">
                    <p class="text-gray-800">${responseMessage}</p>
                </div>
            </div>`;
    chatBox.innerHTML += aiMessage;
    chatBox.scrollTop = chatBox.scrollHeight;
});

const input = document.getElementById("user-input");
const chatBox = document.getElementById("chat-box");

function sendPrompt() {
    if (input.value.trim() === "") return;

    const userMessage = `<div class="flex items-end justify-end">
                <div id="chat-input" class="bg-blue-600 p-3 rounded-lg max-w-xl text-white">
                    <p>${input.value}</p>
                </div>
                <img src="@((userProfilePicture != null) ? userProfilePicture : "~/img/profilepictureSmall.jpeg")" alt="User" class="w-10 h-10 rounded-full ml-3">
            </div>`;
    chatBox.innerHTML += userMessage;
    chatBox.scrollTop = chatBox.scrollHeight;

    fetch("https://localhost:7118/ChatBot/ChatBot", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ prompt: input.value, connectionId: hubConnection.connectionId })
    })
        .then(response => response.json())
        .then(data => console.log("Success : ", data))
        .catch(error => console.error("Error", error));
    input.value = "";
}