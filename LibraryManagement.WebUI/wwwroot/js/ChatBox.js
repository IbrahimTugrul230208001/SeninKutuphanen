var hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/ai-hub")
    .build();

// Toggle chat visibility
const chatBoxContainer = document.getElementById('chatBox');
document.getElementById('openChat').addEventListener('click', () => {
    chatBoxContainer.classList.remove('scale-0', 'opacity-0');
});
document.getElementById('closeChat').addEventListener('click', () => {
    chatBoxContainer.classList.add('scale-0', 'opacity-0');
});

async function startHubConnection() {
    try {
        await hubConnection.start();
        document.getElementById("connectionId").innerText += hubConnection.connectionId;
        console.log("SignalR Connected. Connection ID:", hubConnection.connectionId);
    } catch (err) {
        console.error("SignalR Connection Error:", err.toString());
        setTimeout(startHubConnection, 5000);
    }
}
startHubConnection();

hubConnection.on("ReceiveMessage", (message) => {
    console.log("Received AI Response:", message);

    const chatBox = document.getElementById("chatContent");
    // (Optional) Use marked.parse(message) if you want markdown parsing.
    const modifiedMessage = marked.parse(message);

    // Remove loading
    const loadingMessageBlock = document.getElementById("loading-message");
    if (loadingMessageBlock) {
        loadingMessageBlock.remove(); // Remove from DOM for clarity
    }
    console.log("Removing loading message:", loadingMessageBlock);

    // Create AI avatar/image
    const imgElement = document.createElement('img');
    imgElement.src = '/img/MinikZekaAI-Clean.png'; // or your AI image
    imgElement.className = 'w-8 h-8 mr-2 rounded-full';

    // Message text container
    const messageText = document.createElement('div');
    messageText.className = "bg-gray-100 text-gray-900 rounded-lg p-3 max-w-xs";
    messageText.innerHTML = ""; // Typewriter will fill this

    // Outer container for AI message
    const aiMessage = document.createElement('div');
    aiMessage.className = "flex items-start my-2";
    aiMessage.appendChild(imgElement);
    aiMessage.appendChild(messageText);

    chatBox.appendChild(aiMessage);

    // Typewriter effect
    const typewriter = new Typewriter(messageText, {
        delay: 10,
        cursor: ''
    });

    typewriter
        .typeString(modifiedMessage)
        .start()
        .callFunction(() => {
            chatBox.scrollTop = chatBox.scrollHeight;
        });

    setTimeout(() => {
        chatBox.scrollTop = chatBox.scrollHeight;
    }, 100);
});

// Function to send user input to backend
function sendPrompt() {
    const input = document.getElementById("chatInput");
    const chatBox = document.getElementById("chatContent");
    const text = input.value.trim();
    if (!text) return;

    chatBox.innerHTML += `<div class="my-2 flex justify-end"><div class="bg-yellow-200 px-3 py-2 text-gray-900 max-w-xs rounded-lg">${text}</div></div>`;
    chatBox.scrollTop = chatBox.scrollHeight;
    input.value = "";

    if (!hubConnection.connectionId) {
        console.error("SignalR Connection ID is not available.");
        return;
    }

    // Loading spinner as a DOM element
    const loadingMessageBlock = document.createElement('div');
    loadingMessageBlock.id = "loading-message";
    loadingMessageBlock.className = "flex items-start";
    loadingMessageBlock.innerHTML = `
        <div class="mb-10 p-3 rounded-lg max-w-xl">
            <svg class="animate-spin w-10 h-10 text-yellow-400 mr-10 mb-7" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"/>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v4a4 4 0 00-4 4H4z"/>
            </svg>
        </div>`;
    chatBox.appendChild(loadingMessageBlock);
    chatBox.scrollTop = chatBox.scrollHeight;

    fetch("/Platform/Sohbet", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            prompt: text,
            connectionId: hubConnection.connectionId
        })
    })
        .then(async response => {
            if (!response.ok) {
                console.log("HTTP error:", response.status, response.statusText);
                return;
            }
            const text = await response.text();
            if (!text) {
                console.log("Empty response body");
                return;
            }
            let data;
            try {
                data = JSON.parse(text);
            } catch (e) {
                console.log("Response not valid JSON:", text);
                return;
            }
            // Now use data as before
            if (!data.response) {
                console.log("Invalid API response:", data);
                return;
            }
        });


}


// Only trigger sendPrompt when Enter is pressed inside the chat input
document.getElementById('chatInput').addEventListener('keydown', function (e) {
    if (e.key === 'Enter') {
        sendPrompt();
    }
});
