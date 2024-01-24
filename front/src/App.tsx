import InputLabel from "@mui/material/InputLabel";
import { useState } from "react";

import { UsernamePage, LandingPage } from "./pages";

import "./App.css";

function App() {
  const [username, setUsername] = useState<null | String>(null);

  return (
    <div className="App">
      {username ? <LandingPage /> : <UsernamePage setUsername={setUsername} />}
    </div>
  );
}

export default App;
