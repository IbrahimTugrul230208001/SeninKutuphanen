// Initialize SignalR connection
var hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7254/ai-hub")
    .build();

// Start the connection and ensure it's ready
async function startHubConnection() {
    try {
        await hubConnection.start();
        document.getElementById("connectionId").innerText = hubConnection.connectionId;
        console.log("SignalR Connected. Connection ID:", hubConnection.connectionId);
    } catch (err) {
        console.error("SignalR Connection Error:", err.toString());
        setTimeout(startHubConnection, 5000); // Retry connection every 5 seconds
    }
}
startHubConnection(); // Start connection on page load

// Function to send the user input
function sendPrompt() {
    var input = document.getElementById("user-input");
    var chatBox = document.getElementById("chat-box");

    if (input.value.trim() === "") return; // Don't send empty messages

    // ✅ Save the input value before clearing
    const userMessageText = input.value;

    // 1. Add User's Message to Chat Box
    const userMessage = `
        <div class="flex items-end justify-end">
            <div id="chat-input" class="bg-blue-600 p-3 rounded-lg max-w-xl text-white">
                <p>${userMessageText}</p>
            </div>
        </div>`;
    chatBox.innerHTML += userMessage;
    chatBox.scrollTop = chatBox.scrollHeight; // Scroll to the bottom

    // 2. Clear the input field
    input.value = "";

    // 3. Ensure the connection ID is available
    if (!hubConnection.connectionId) {
        console.error("SignalR Connection ID is not available.");
        return;
    }

    // 4. Send the prompt to backend
    console.log("Sending Prompt:", userMessageText);
    console.log("Connection ID:", hubConnection.connectionId);

    fetch("/ChatBot/Chat", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            prompt: userMessageText, // ✅ Use stored value
            connectionId: hubConnection.connectionId
        })
    })
        .then(response => response.json())
        .then(data => {
            if (!data.response) {
                console.error("Invalid API response:", data);
                return;
            }

            // 5. Add AI Response to Chat Box
            const aiMessage = `
        <div class="flex items-start">
            <img src="/img/logo.png" alt="Bot" class="w-10 h-10 rounded-full mr-3">
            <div class="bg-gray-100 p-3 rounded-lg max-w-xl">
                <p class="text-gray-800">${data.response}</p> <!-- Assuming response is in 'response' -->
            </div>
        </div>`;
            chatBox.innerHTML += aiMessage;
            chatBox.scrollTop = chatBox.scrollHeight; // Scroll to the bottom
        })
        .catch(error => console.error("Error:", error));
}

