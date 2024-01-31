import { FC, useState } from "react";
import classes from "classnames";
import TextField from "@mui/material/TextField";
import AddIcon from "@mui/icons-material/Add";
import GetAppIcon from "@mui/icons-material/GetApp";

import "./GenresForm.styles.scss";
import { Button } from "@mui/material";
import useSnackbar from "../../../Hooks/useSnackbar";
import axios from "axios";
import { CREATE_GENRE } from "../../../endpoints";

type GenresFormProps = {
  handleLoadGenres: () => void;
  className?: String;
};

const GenresForm: FC<GenresFormProps> = ({ className, handleLoadGenres }) => {
  const classNames = classes("genres-form", className);

  const { createSnackbar } = useSnackbar({
    message: "Successfully created genre",
    errorMessage: "Request failed",
  });

  const [name, setName] = useState("");

  const handleCreateGenre = async () => {
    if (name === "") return;

    axios
      .post(CREATE_GENRE, { name })
      .then(() => {
        createSnackbar({ error: false });
      })
      .catch(() => {
        createSnackbar({ error: true });
      });
  };
  return (
    <div className={classNames}>
      <h2 className="genres-form__title">Create new genre</h2>
      <div className="genres-form__field">
        <TextField
          variant="filled"
          label="Genre name"
          className="genres-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "235px" }}
          value={name}
          onChange={(e) => {
            setName(e.target.value);
          }}
        />
        <Button
          variant="contained"
          endIcon={<AddIcon />}
          className="genres-form__field__button"
          onClick={handleCreateGenre}
        >
          create
        </Button>
      </div>
      <div className="genres-form__field">
        <Button
          variant="contained"
          endIcon={<GetAppIcon />}
          className="genres-form__field__button"
          sx={{ width: "100%", fontSize: "18px" }}
          onClick={handleLoadGenres}
        >
          Get all genres
        </Button>
      </div>
    </div>
  );
};

export default GenresForm;
