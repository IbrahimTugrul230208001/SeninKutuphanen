// Initialize SignalR connection
var hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7254/ai-hub")
    .build();

// Start the connection and set connection ID in the view
hubConnection.start()
    .then(() =>
        document.getElementById("connectionId").innerText = hubConnection.connectionId
    )
    .catch(err =>
        console.error(err.toString()));

// Function to send the user input
function sendPrompt() {
    var input = document.getElementById("user-input");
    var chatBox = document.getElementById("chat-box");

    if (input.value.trim() === "") return; // Don't send empty messages

    // 1. Add User's Message to Chat Box
    const userMessage = `
        <div class="flex items-end justify-end">
            <div id="chat-input" class="bg-blue-600 p-3 rounded-lg max-w-xl text-white">
                <p>${input.value}</p>
            </div>
        </div>`;
    chatBox.innerHTML += userMessage;
    chatBox.scrollTop = chatBox.scrollHeight; // Scroll to the bottom

    // 2. Clear the input field
    input.value = "";

    // 3. Send the prompt to backend and wait for AI response
    fetch("/chat", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            prompt: input.value,
            connectionId: hubConnection.connectionId
        })
    })
        .then(response => response.json())
        .then(data => {
            // 4. Add AI Response to Chat Box
            const aiMessage = `
            <div class="flex items-start">
                <img src="/img/logo.png" alt="Bot" class="w-10 h-10 rounded-full mr-3">
                <div class="bg-gray-100 p-3 rounded-lg max-w-xl">
                    <p class="text-gray-800">${data.response}</p> <!-- Assuming the response is in 'response' -->
                </div>
            </div>`;
            chatBox.innerHTML += aiMessage;
            chatBox.scrollTop = chatBox.scrollHeight; // Scroll to the bottom
        })
        .catch(error => console.error("Error:", error));
}
