import { useState } from "react";

const initialMessages = [
    { id: 1, title: "New Tip", content: "Consistently add 2.5 lbs to bench to reach your 315 lb goal!" },
    { id: 2, title: "New Tip", content: "Stay Hydrated!" },
  { id: 3, title: "Weekly Goal Reminder", content: "Don't forget your weekly workout goal!" },
  { id: 4, title: "New Tip", content: "Try stretching 5 minutes before every session." },
  { id: 5, title: "New Tip", content: "Eat protein rich foods to reach goal weight!" },
  { id: 6, title: "Welcome!", content: "You're all set up with SmartGymTracker." },
];

export default function SmartTips() {


  const [messages, setMessages] = useState(initialMessages);
  const [unread, setUnread] = useState([1, 2, 4]); // store unread message IDs
  const [tab, setTab] = useState("inbox"); // "inbox" or "unread"

  const markAsRead = (id) => {
    setUnread(unread.filter(u => u !== id)); // remove from unread
  };

  const deleteTip = (id) => {
    setMessages(messages.filter(m => m.id !== id)); // remove from all messages
    setUnread(unread.filter(u => u !== id)); // remove from unread if present
  };

  const currentList = tab === "inbox"
    ? messages
    : messages.filter(m => unread.includes(m.id)); // only unread messages

  return (
    <div className="p-6">
      <h1 className="text-3xl font-bold mb-6 text-center">Smart Tips & Messages</h1>

      {/* Tabs */}
      <div className="flex justify-center gap-4 mb-6">
        <button
          className={`px-4 py-2 rounded ${tab === "inbox" ? "bg-blue-600 text-white" : "bg-gray-200"}`}
          onClick={() => setTab("inbox")}
        >
          Current Inbox
        </button>
        <button
          className={`px-4 py-2 rounded ${tab === "unread" ? "bg-blue-600 text-white" : "bg-gray-200"}`}
          onClick={() => setTab("unread")}
        >
          Unread
        </button>
      </div>

      {/* Messages */}
      <div className="flex flex-col gap-4 pb-6">
        {currentList.length === 0 && <p className="text-gray-500 text-center">No messages here!</p>}
        {currentList.map((tip) => {
          const isUnread = unread.includes(tip.id);
          return (
            <div key={tip.id} className="bg-white p-4 rounded shadow-md flex justify-between items-center">
              <div className="flex items-center gap-2">
                {/* Blue dot only if unread */}
                {isUnread && <div className="w-3 h-3 bg-blue-500 rounded-full" />}
                <div>
                  <h2 className="font-semibold text-lg">{tip.title}</h2>
                  <p className="text-gray-700">{tip.content}</p>
                </div>
              </div>

              <div className="flex gap-2">
                {tab === "unread" && isUnread && (
                  <button
                    onClick={() => markAsRead(tip.id)}
                    className="bg-green-600 text-white px-3 py-1 rounded hover:bg-green-700"
                  >
                    Mark as Read
                  </button>
                )}
                {tab === "inbox" && (
                  <button
                    onClick={() => deleteTip(tip.id)}
                    className="text-red-500 font-bold px-2 hover:text-red-700"
                  >
                    ✕
                  </button>
                )}
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}

