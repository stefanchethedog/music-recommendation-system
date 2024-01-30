import { FC } from "react";
import classes from "classnames";
import TextField from "@mui/material/TextField";
import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";
import GetAppIcon from "@mui/icons-material/GetApp";

import useSnackbar from "../../../Hooks/useSnackbar";
import { useForm } from "react-hook-form";
import axios from "axios";
import { CREATE_ALBUM } from "../../../endpoints";

import "./AlbumsForm.styles.scss";

type AlbumsFormProps = {
  handleLoadGenres: () => void;
  className?: String;
};

interface ICreateAlbum {
  name: String;
  authorName: String;
  genres: String;
  songs: String;
}

const AlbumsForm: FC<AlbumsFormProps> = ({ className, handleLoadGenres }) => {
  const classNames = classes("albums-form", className);

  const { createSnackbar } = useSnackbar({
    message: "All gucci",
    errorMessage: "Not all gucci",
  });

  const { register, handleSubmit } = useForm<ICreateAlbum>({
    defaultValues: {
      name: "",
      authorName: "",
      genres: "",
      songs: "",
    },
  });

  const onSubmit = (data: ICreateAlbum) => {
    const payload = {
      name: data.name,
      authorName: data.authorName,
      genres: data.genres.split(", "),
      songs: data.songs.split(", "),
    };
    axios
      .post(CREATE_ALBUM, payload)
      .then((res) => {
        createSnackbar({ error: false });
      })
      .catch(() => {
        createSnackbar({ error: true });
      });
  };

  return (
    <div className={classNames}>
      <h2 className="albums-form__title">Create a new album</h2>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="albums-form__field">
          <TextField
            variant="filled"
            label="Title"
            className="albums-form__field__text-field"
            sx={{
              backgroundColor: "white",
              borderRadius: "4px",
              width: "375px",
            }}
            {...register("name", { required: true })}
          />
        </div>
        <div className="albums-form__field">
          <TextField
            variant="filled"
            label="Author"
            className="albums-form__field__text-field"
            sx={{
              backgroundColor: "white",
              borderRadius: "4px",
              width: "375px",
            }}
            {...register("authorName", { required: true })}
          />
        </div>
        <div className="albums-form__field">
          <TextField
            variant="filled"
            label="Genres"
            className="albums-form__field__text-field"
            sx={{
              backgroundColor: "white",
              borderRadius: "4px",
              width: "375px",
            }}
            {...register("genres", { required: true })}
          />
        </div>
        <div className="albums-form__field">
          <TextField
            variant="filled"
            label="Songs"
            className="albums-form__field__text-field"
            sx={{
              backgroundColor: "white",
              borderRadius: "4px",
              width: "375px",
            }}
            {...register("songs", { required: true })}
          />
        </div>
        <div className="albums-form__field">
          <Button
            variant="contained"
            className="albums-form__field__button"
            endIcon={<AddIcon />}
            sx={{ width: "100%", fontSize: "18px" }}
            type="submit"
          >
            Create album
          </Button>
        </div>
      </form>
      <div className="albums-form__field">
          <Button
            variant="contained"
            endIcon={<GetAppIcon />}
            className="albums-form__field__button"
            sx={{ width: "100%", fontSize: "18px" }}
            onClick={handleLoadGenres}
          >
            Get albums
          </Button>
        </div>
    </div>
  );
};

export default AlbumsForm;
