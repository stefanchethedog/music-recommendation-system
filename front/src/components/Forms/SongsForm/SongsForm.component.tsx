import { FC } from "react";
import classes from "classnames";
import TextField from "@mui/material/TextField";
import AddIcon from "@mui/icons-material/Add";
import GetAppIcon from "@mui/icons-material/GetApp";

import "./SongsForm.styles.scss";
import { Button } from "@mui/material";
import { useForm } from "react-hook-form";
import axios from "axios";
import { CREATE_SONG } from "../../../endpoints";
import useSnackbar from "../../../Hooks/useSnackbar";

type SongsFormProps = {
  handleLoadSongs: () => void;
  className?: String;
};

interface ICreateSong {
  name: string;
  artist: string;
  genres: string;
  album?: string;
}

const SongsForm: FC<SongsFormProps> = ({ className, handleLoadSongs }) => {
  const classNames = classes("songs-form", className);

  const { createSnackbar } = useSnackbar({
    message: "All good",
    errorMessage: "Not all gucci",
  });

  const { register, handleSubmit } = useForm<ICreateSong>({
    defaultValues: {
      name: "",
      artist: "",
      genres: "",
    },
  });

  const onSubmit = async (data: ICreateSong) => {
    let payload;
    if (data.album === "") {
      payload = {
        name: data.name,
        author: data.artist,
        genres: data.genres.split(", "),
      };
    } else {
      payload = {
        name: data.name,
        author: data.artist,
        album: data.album,
        genres: data.genres.split(", "),
      };
    }

    await axios
      .post(CREATE_SONG, payload)
      .then((res) => {
        console.log(res);
        createSnackbar({ error: false });
      })
      .catch((err) => {
        createSnackbar({ error: true });
        console.log(err);
      });
  };

  return (
    <div className={classNames}>
      <h2 className="songs-form__title">Create a new song</h2>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="songs-form__field">
          <TextField
            variant="filled"
            label="Title"
            className="songs-form__field__text-field"
            sx={{
              backgroundColor: "white",
              borderRadius: "4px",
              width: "375px",
            }}
            {...register("name", { required: true })}
          />
        </div>

        <div className="songs-form__field">
          <TextField
            variant="filled"
            label="Artist"
            className="songs-form__field__text-field"
            sx={{
              backgroundColor: "white",
              borderRadius: "4px",
              width: "375px",
            }}
            {...register("artist", { required: true })}
          />
        </div>
        <div className="songs-form__field">
          <TextField
            variant="filled"
            label="Album"
            className="songs-form__field__text-field"
            sx={{
              backgroundColor: "white",
              borderRadius: "4px",
              width: "375px",
            }}
            {...register("album", { required: false })}
          />
        </div>
        <div className="songs-form__field">
          <TextField
            variant="filled"
            label="Genres"
            className="songs-form__field__text-field"
            sx={{
              backgroundColor: "white",
              borderRadius: "4px",
              width: "375px",
            }}
            {...register("genres", { required: true })}
          />
        </div>
        <div className="songs-form__field">
          <Button
            variant="contained"
            endIcon={<AddIcon />}
            className="user-form__field__button"
            sx={{ width: "100%", fontSize: "18px" }}
            type="submit"
          >
            create
          </Button>
        </div>
      </form>
      <div className="songs-form__field">
          <Button
            variant="contained"
            endIcon={<GetAppIcon />}
            className="user-form__field__button"
            sx={{ width: "100%", fontSize: "18px" }}
            onClick={handleLoadSongs}
          >
            Get songs
          </Button>
        </div>
    </div>
  );
};

export default SongsForm;
