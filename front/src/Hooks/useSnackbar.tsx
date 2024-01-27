import { Snackbar } from "../components";
import ReactDOM from "react-dom";

type SnackbarProps = {
  message: String;
  errorMessage: String;
};

const useSnackbar = ({ message, errorMessage }: SnackbarProps) => {

  const createSnackbar = ({ error = false }) => {

    const root = document.getElementById("root") as HTMLElement;
    const snackbar = document.createElement("div");
    snackbar.setAttribute("id", "snackbar-id");
    root.appendChild(snackbar);

    ReactDOM.render(
      <Snackbar
        message={!error ? message : errorMessage}
        handleOnClick={closeSnackbar}
        id="snackbar"
        className={error ? "snackbar__error" : ""}
      ></Snackbar>,
      snackbar
    );
  };

  const closeSnackbar = () => {
    const element = document.getElementById("snackbar-id") as HTMLElement;
    const root = document.getElementById("root") as HTMLElement;

    root.removeChild(element);
  };

  return { createSnackbar, closeSnackbar };
};

export default useSnackbar;
