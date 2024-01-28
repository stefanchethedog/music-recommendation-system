import { FC } from "react";
import classes from "classnames";
import TextField from "@mui/material/TextField";
import AddIcon from "@mui/icons-material/Add";
import BookmarkIcon from "@mui/icons-material/Bookmark";
import ThumbUpOffAltIcon from "@mui/icons-material/ThumbUpOffAlt";
import StarsIcon from "@mui/icons-material/Stars";

import "./ArtistForm.styles.scss";
import { Button } from "@mui/material";

type ArtistFormProps = {
  className?: String;
};

const ArtistForm: FC<ArtistFormProps> = ({ className }) => {
  const classNames = classes("artist-form", className);
  return (
    <div className={classNames}>
      <h2 className="artist-form__title">Artist</h2>
      <div className="artist-form__field">
        <TextField
          variant="filled"
          label="Artist"
          className="artist-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "230px" }}
        />
        <Button
          variant="contained"
          endIcon={<AddIcon />}
          className="artist-form__field__button"
        >
          create
        </Button>
      </div>
      <div className="artist-form__field">
        <Button
          variant="contained"
          className="artist-form__field__button"
          endIcon={<StarsIcon />}
          startIcon={<StarsIcon />}
          sx={{
            width: "100%",
            height: "50px",
          }}
        >
          Get artists
        </Button>
      </div>
    </div>
  );
};

export default ArtistForm;
