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
        setTimeout(startHubConnection, 5000);
    }
}
startHubConnection();

hubConnection.on("ReceiveMessage", (message) => {
    console.log("Received AI Response:", message);

    const chatBox = document.getElementById("chat-box");
    // Parse and format the message (if you're using a markdown library)
    const modifiedMessage = marked.parse(message);

    // Remove the loading placeholder if it exists
    const loadingMessageBlock = document.getElementById("loading-message");
    if (loadingMessageBlock) {
        loadingMessageBlock.remove();
    }

    // Create the final AI message structure
    const aiMessage = document.createElement('div');
    aiMessage.classList.add("flex", "items-start");

    const imgElement = document.createElement('img');
    imgElement.src = "/img/logo.png";
    imgElement.alt = "Bot";
    imgElement.classList.add("w-10", "h-10", "rounded-full", "mr-3");

    const messageContainer = document.createElement('div');
    messageContainer.classList.add("bg-gray-100", "p-3", "rounded-lg", "max-w-xl");

    const messageText = document.createElement('p');
    messageText.classList.add("text-gray-800");
    messageText.innerHTML = ""; // Start with an empty string

    messageContainer.appendChild(messageText);
    aiMessage.appendChild(imgElement);
    aiMessage.appendChild(messageContainer);

    chatBox.appendChild(aiMessage);  // Add message container to chat box

    // Initialize the typewriter effect on the message text
    const typewriter = new Typewriter(messageText, {
        delay: 10,
        cursor: ''
    });

    typewriter.typeString(modifiedMessage).start().callFunction(() => {
        chatBox.scrollTop = chatBox.scrollHeight;  // Scroll after typing is done
    });

    // Ensure the scroll is updated after the AI's response
    setTimeout(() => {
        chatBox.scrollTop = chatBox.scrollHeight;
    }, 100); // Add a small delay to ensure scrolling happens after message is rendered
});

// Function to send user input to backend
function sendPrompt() {
    var input = document.getElementById("user-input");
    var chatBox = document.getElementById("chat-box");

    if (input.value.trim() === "") return; // Don't send empty messages

    // Save the input value before clearing
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

    // 4. Add a loading message placeholder
    const loadingMessageBlock = document.createElement('div');
    loadingMessageBlock.id = "loading-message";
    loadingMessageBlock.classList.add("flex", "items-start");

    const aiImage = document.createElement('img');
    aiImage.src = "/img/logo.png";
    aiImage.alt = "Bot";
    aiImage.classList.add("w-10", "h-10", "rounded-full", "mr-3");

    const loadingContainer = document.createElement('div');
    loadingContainer.classList.add("mb-10", "p-3", "rounded-lg", "max-w-xl");

    // Create and add the loading GIF image
    const gifImage = document.createElement('img');
    gifImage.src = "/img/gif-5-unscreen.gif"; // Replace with the actual path to your loading gif
    gifImage.alt = "Loading...";
    gifImage.classList.add("w-12", "h-12", "mr-10", "mb-7");
    gifImage.classList.add("loading-gif");

    loadingContainer.appendChild(gifImage);
    loadingMessageBlock.appendChild(aiImage);
    loadingMessageBlock.appendChild(loadingContainer);
    chatBox.appendChild(loadingMessageBlock);
    chatBox.scrollTop = chatBox.scrollHeight;

    // 5. Send the prompt to backend
    console.log("Sending Prompt:", userMessageText);
    console.log("Connection ID:", hubConnection.connectionId);

    fetch("/ChatBot/Chat", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            prompt: userMessageText,
            connectionId: hubConnection.connectionId
        })
    })
        .then(response => response.json())
        .then(data => {
            if (!data.response) {
                console.error("Invalid API response:", data);
                return;
            }
            // Optionally, you could handle the response here too,
            // but we will update the placeholder in the SignalR callback.
        })
        .catch(error => console.error("Error:", error));
}
