import { FC } from "react";
import classes from "classnames";
import TextField from "@mui/material/TextField";
import AddIcon from "@mui/icons-material/Add";

import "./SongsForm.styles.scss";
import { Button } from "@mui/material";

type SongsFormProps = {
  className?: String;
};

const SongsForm: FC<SongsFormProps> = ({ className }) => {
  const classNames = classes("songs-form", className);
  //U formi da se stavi ovo
  return (
    <div className={classNames}>
      <h2 className="songs-form__title">Create a new song</h2>
      <div className="songs-form__field">
        <TextField
          variant="filled"
          label="Title"
          className="songs-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "375px" }}
        />
      </div>

      <div className="songs-form__field">
        <TextField
          variant="filled"
          label="Artist"
          className="songs-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px",  width: "375px" }}
        />
      </div>
      <div className="songs-form__field">
        <TextField
          variant="filled"
          label="Album"
          className="songs-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "375px" }}
        />
      </div>
      <div className="songs-form__field">
        <TextField
          variant="filled"
          label="Genres"
          className="songs-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "375px" }}
        />
      </div>
      <div className="songs-form__field">
        <Button
            variant="contained"
            endIcon={<AddIcon/>}
            className="user-form__field__button"
            sx = {{ width: '100%', fontSize:'18px'}}
          >
          create
        </Button>
      </div>
    </div>
  );
};

export default SongsForm;
