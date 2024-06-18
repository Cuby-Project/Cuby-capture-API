from rubik_solver import utils


def solve(cube: str) -> str:
    try:
        return utils.solve(cube)
    except Exception as N:
        return "Invalid cube"


def verify(cube: str) -> bool:
    return solve(cube) == cube
