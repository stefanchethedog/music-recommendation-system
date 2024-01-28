import { FC } from "react";
import classes from "classnames";
import TextField from "@mui/material/TextField";
import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";

import "./AlbumsForm.styles.scss";

type AlbumsFormProps = {
  className?: String;
};

const AlbumsForm: FC<AlbumsFormProps> = ({ className }) => {
  const classNames = classes("albums-form", className);
  return (
    <div className={classNames}>
      <h2 className="albums-form__title">Create a new album</h2>
      <div className="albums-form__field">
        <TextField
          variant="filled"
          label="Title"
          className="albums-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "375px" }}
        />
      </div>
      <div className="albums-form__field">
        <TextField
          variant="filled"
          label="Author"
          className="albums-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "375px" }}
        />
      </div>
      <div className="albums-form__field">
        <TextField
          variant="filled"
          label="Genres"
          className="albums-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "375px" }}
        />
      </div>
      <div className="albums-form__field">
        <TextField
          variant="filled"
          label="Songs"
          className="albums-form__field__text-field"
          sx={{ backgroundColor: "white", borderRadius: "4px", width: "375px" }}
        />
      </div>
      <div className="albums-form__field">
        <Button
          variant="contained"
          className="albums-form__field__button"
          endIcon={<AddIcon />}
          sx = {{ width: '100%', fontSize:'18px'}}
        >
          Create album
        </Button>
      </div>
    </div>
  );
};

export default AlbumsForm;
