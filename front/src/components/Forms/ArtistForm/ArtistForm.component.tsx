import { FC, useState } from "react";
import classes from "classnames";
import TextField from "@mui/material/TextField";
import AddIcon from "@mui/icons-material/Add";
import GetAppIcon from "@mui/icons-material/GetApp";

import "./ArtistForm.styles.scss";
import { Button } from "@mui/material";
import { CREATE_ARTIST } from "../../../endpoints";
import axios from "axios";
import useSnackbar from "../../../Hooks/useSnackbar";

type ArtistFormProps = {
  handleLoadArtists: () => void;
  className?: String;
};

const ArtistForm: FC<ArtistFormProps> = ({ className, handleLoadArtists }) => {
  const classNames = classes("artist-form", className);
  const { createSnackbar } = useSnackbar({
    message: "Successfully created artist",
    errorMessage: "Error",
  });

  const [name, setName] = useState("");

  const handleCreateArtist = async () => {
    if (name === "") return;
    await axios
      .post(CREATE_ARTIST, { name })
      .then(() => {
        createSnackbar({ error: false });
      })
      .catch(() => {
        createSnackbar({ error: true });
      });
  };

  return (
    <div className={classNames}>
      <h2 className="artist-form__title">Artist</h2>
      <div className="artist-form__field">
        <TextField
          variant="filled"
          label="Artist"
          className="artist-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "230px" }}
          onChange={(e) => {
            setName(e.target.value);
          }}
          value={name}
        />
        <Button
          variant="contained"
          endIcon={<AddIcon />}
          className="artist-form__field__button"
          onClick={handleCreateArtist}
        >
          create
        </Button>
      </div>
      <div className="artist-form__field">
        <Button
          variant="contained"
          className="artist-form__field__button"
          endIcon={<GetAppIcon />}
          sx={{
            width: "100%",
            height: "50px",
          }}
          onClick={handleLoadArtists}
        >
          Get artists
        </Button>
      </div>
    </div>
  );
};

export default ArtistForm;
